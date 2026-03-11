# BCKFreight TMS – Copilot Instructions

## Build & Test Commands

```bash
# Build the solution
dotnet build src/BCKFreightTMS.sln

# Run all tests
dotnet test src/BCKFreightTMS.sln

# Run a single test class
dotnet test src/Tests/BCKFreightTMS.Services.Data.Tests --filter "FullyQualifiedName~ContactsServiceTests"

# Run a single test method
dotnet test src/Tests/BCKFreightTMS.Services.Data.Tests --filter "FullyQualifiedName~ContactsServiceTests.AddCompanyTest"

# Run the web app (default: https://localhost:44319)
dotnet run --project src/Web/BCKFreightTMS.Web/BCKFreightTMS.Web.csproj

# Add EF Core migration
dotnet ef migrations add <MigrationName> --project src/Data/BCKFreightTMS.Data --startup-project src/Web/BCKFreightTMS.Web

# Apply migrations
dotnet ef database update --project src/Data/BCKFreightTMS.Data --startup-project src/Web/BCKFreightTMS.Web
```

## Architecture

**Solution:** `src/BCKFreightTMS.sln` — .NET 10, ASP.NET Core MVC, SQL Server

```
BCKFreightTMS.Common              # GlobalConstants, enums, shared utilities
Data/
  BCKFreightTMS.Data.Models       # EF Core entity classes
  BCKFreightTMS.Data.Common       # Base models, repository interfaces
  BCKFreightTMS.Data              # ApplicationDbContext, EF configurations, migrations
Services/
  BCKFreightTMS.Services          # Base/infrastructure service interfaces
  BCKFreightTMS.Services.Data     # Business logic service implementations
  BCKFreightTMS.Services.Mapping  # AutoMapper configuration (reflection-based)
  BCKFreightTMS.Services.Messaging# Email sending
Web/
  BCKFreightTMS.Web.ViewModels    # Input/view models, DTO contracts
  BCKFreightTMS.Web               # ASP.NET Core MVC app, controllers, views
Tests/
  BCKFreightTMS.Services.Data.Tests  # Service unit tests (xUnit + Moq + InMemory EF)
  BCKFreightTMS.Web.Tests            # Web/integration tests (Selenium)
  Sandbox                            # CLI sandbox tool
```

**Request flow:** Controller → `IXxxService` → `IDeletableEntityRepository<TEntity>` → `ApplicationDbContext`

## Key Conventions

### Base Entity Models

All data models inherit from one of two abstract base classes in `Data/BCKFreightTMS.Data.Common/Models/`:

- **`BaseModel<TKey>`** — provides `Id`, `CreatedOn`, `ModifiedOn`, `AdminId` (company scoping)
- **`BaseDeletableModel<TKey>`** — extends `BaseModel<TKey>`, adds `IsDeleted` + `DeletedOn` for soft delete

Most domain entities use `BaseDeletableModel<string>` (string GUID keys).

### Soft Delete

- `IDeletableEntityRepository<T>.Delete()` sets `IsDeleted = true` + `DeletedOn = DateTime.UtcNow`
- `ApplicationDbContext` applies a global EF query filter (`!IsDeleted`) on all `IDeletableEntity` types automatically
- Use `AllWithDeleted()` / `AllAsNoTrackingWithDeleted()` to bypass the filter
- Use `HardDelete()` only when a true physical delete is required
- All FK cascade deletes are globally disabled — set to `DeleteBehavior.Restrict`

### Repository Pattern

Always inject `IDeletableEntityRepository<TEntity>` (not `IRepository<TEntity>`) for deletable entities:

```csharp
// Preferred
private readonly IDeletableEntityRepository<Order> orders;

// Key methods
.All()                      // non-deleted, tracked
.AllAsNoTracking()          // non-deleted, untracked (use for reads)
.AllWithDeleted()           // includes deleted
.HardDelete(entity)         // physical delete
```

### Service Layer

- Interfaces: `I{EntityName}Service` in `Services.Data`
- Implementations: `{EntityName}Service` in `Services.Data`
- Services receive repository interfaces via constructor DI and use `IMapper` for projections
- Return `string` (ID) from create/edit operations, `bool` from delete, `IEnumerable<T>` (projected via AutoMapper) from queries

### AutoMapper (Reflection-Based Registration)

Mappings auto-register via `AutoMapperConfig.RegisterMappings(assemblies)` at startup. Add mappings by implementing:

- **`IMapFrom<TSource>`** — on a ViewModel to auto-map from the source entity (no code needed)
- **`IMapTo<TDestination>`** — on a model to auto-map to the destination
- **`IHaveCustomMappings`** — implement `CreateMappings(IProfileExpression)` for complex/flattened mappings

```csharp
// Simple mapping
public class SettingViewModel : IMapFrom<SettingModel>
{
    public int Id { get; set; }
    public string Name { get; set; }
}

// Custom mapping
public class ListOrderViewModel : IMapFrom<Order>, IHaveCustomMappings
{
    public string Voyage { get; set; }

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<Order, ListOrderViewModel>()
            .ForMember(x => x.Voyage, opt => opt.MapFrom(x => ...));
    }
}
```

### ViewModel Naming

- List/display: `{Entity}ViewModel` or `List{Entity}ViewModel`
- Create/edit input: `{Entity}CreateInputModel` / `{Entity}EditInputModel`
- Accept/action: `{Entity}AcceptInputModel`

### Controllers

- All controllers inherit `BaseController` which applies `[Authorize]`
- Role-based access: `[Authorize(Roles = "User")]` or `[Authorize(Roles = "Administrator")]`
- Administration features live in `Areas/Administration/`
- Use `INotyfService` for toast notifications

### Company / Multi-Tenancy Scoping

Every entity has `AdminId` (via `ICompanyEntity`). The `EfRepository` automatically scopes queries to the current user's `AdminId`. Do not filter by `AdminId` manually in services — the repository handles it.

### Constants & Enums

- All string/numeric constants → `BCKFreightTMS.Common/GlobalConstants.cs`
- Domain string-constant groups (e.g., reasons, labels) → sealed singleton classes in `BCKFreightTMS.Common`
- Enumerated domain concepts → `BCKFreightTMS.Common/Enums/` (one file per enum)

### EF Core Configuration

Entity relationships are configured via `IEntityTypeConfiguration<T>` classes in `BCKFreightTMS.Data/Configurations/`. All configurations auto-apply via `builder.ApplyConfigurationsFromAssembly(...)`. Decimal columns use explicit precision: `HasPrecision(18, 2)` for money, `HasPrecision(18, 6)` for rates.

### Tests

- Framework: **xUnit** with **Moq** and **EF Core InMemory** provider
- Each test method creates a fresh `ApplicationDbContext` (in-memory)
- Use `RepositoryFactory` helper to create `EfDeletableEntityRepository<T>` instances with mocked `IHttpContextAccessor` and `UserManager`
- Test class names follow `{ServiceName}Tests` convention

### Code Style (StyleCop + EditorConfig)

- `using` directives go **inside** the namespace
- System usings come first, with a blank line before other groups
- Newline required at end of file
- Hungarian prefixes allowed: `db`, `at`, `or`, `up`, `it`, `un`, `x`, `y`, `id`, `ip`, `bg`
- StyleCop SA* and Roslyn CA* rules enforced via `Rules.ruleset` — build warnings are treated seriously
