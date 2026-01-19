# .NET 6.0 to .NET 10.0 Migration Plan

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Project-by-Project Migration Plans](#project-by-project-migration-plans)
- [Risk Management](#risk-management)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scope Overview

**Upgrade Target**: .NET 6.0 ? .NET 10.0  
**Repository**: BCKFreight TMS  
**Total Projects**: 13 (all SDK-style, modern format)  
**Total Issues Found**: 127 (18 mandatory, 109 potential)  
**Affected Files**: 25 of 430 total files  
**Estimated LOC to Review**: 74+ lines  

### Current State
- All 13 projects currently target `.NET 6.0` (released November 2021)
- Well-structured dependency hierarchy with 4 foundation libraries and 2 application endpoints
- Modern SDK-style project format throughout (no legacy .csproj files)
- Strong test coverage with dedicated test projects

### Target State
- All 13 projects targeting `.NET 10.0` (Long-Term Support, released November 2024)
- 16 NuGet packages require version updates to `10.0.2`
- 5 packages have functionality now included in framework (will be removed)
- System.Drawing API migration needed (44 issues concentrated in image processing)

### Selected Strategy
**All-At-Once Strategy** - All projects upgraded simultaneously in single atomic operation

**Rationale**:
- 13 projects is manageable for coordinated upgrade
- Clear linear dependency structure (6 levels, no circular dependencies)
- All projects have similar maturity and SDK-style format
- Simplifies testing: single comprehensive validation after upgrade
- Faster timeline: no intermediate checkpoints or phased coordination needed
- Risk is medium and manageable given structure

### Critical Issues Summary

#### 1. System.Drawing API Migration (44 issues - 35% of complexity)
**Impact**: Images, PDF conversion, graphics operations  
**Affected Projects**: BCKFreightTMS.Services (39 issues), BCKFreightTMS.Web (5 issues)  
**Risk Level**: Medium  
**Mitigation**: System.Drawing.Common NuGet package available; consider alternatives (SkiaSharp, ImageSharp) for new code

#### 2. ASP.NET Core API Changes (61 source incompatibilities)
**Impact**: Web hosting model, Identity setup, middleware configuration  
**Affected Projects**: BCKFreightTMS.Web, BCKFreightTMS.Web.Tests, Sandbox  
**Risk Level**: Medium  
**Mitigation**: Covered by breaking changes catalog (see section below)

#### 3. Behavioral Changes (13 identified)
**Impact**: Uri constructor, HttpContent, XmlSerializer, JsonDocument  
**Affected Projects**: Multiple  
**Risk Level**: Low  
**Mitigation**: Testing will catch runtime changes

### Package Update Summary

| Status | Count | Action |
|--------|-------|--------|
| ? Compatible (no update needed) | 23 | Keep as-is |
| ?? Upgrade to 10.0.2 | 16 | Update all |
| ??? Remove (now in framework) | 5 | Remove references |
| **Total** | **39** | |

**Key Framework Updates**: Entity Framework Core, ASP.NET Core, Extensions, Identity packages all moving from 6.0.0 to 10.0.2

### Success Criteria
- ? All 13 projects target net10.0
- ? All 16 package updates applied
- ? Solution builds with 0 errors
- ? All unit tests pass
- ? No security vulnerabilities
- ? System.Drawing migration completed (or scheduled for post-upgrade review)

---

## Migration Strategy

### All-At-Once Strategy Overview

This migration uses an **all-at-once approach** where all 13 projects are upgraded simultaneously in a single coordinated operation. There are no intermediate phases or checkpoints - the entire solution is updated, built, and tested as one atomic change.

### Why All-At-Once for This Solution

| Factor | Why It Works |
|--------|-------------|
| **Project Count** | 13 projects is optimal for all-at-once (manageable scope) |
| **Dependency Structure** | Clear linear hierarchy with no circular dependencies |
| **Project Maturity** | All SDK-style; no legacy mixed-format complexity |
| **Test Coverage** | Dedicated test projects indicate strong coverage |
| **API Changes** | Concentrated in 3 projects; others are mostly framework updates |
| **Package Updates** | All updates are straightforward compatibility moves |

### Execution Approach

**Phase 0: Prerequisites** (if needed)
- Verify .NET 10 SDK installation
- Validate global.json (if present) - no breaking changes expected

**Phase 1: Atomic Upgrade** (single operation, no checkpoints)
1. Update all 13 project files simultaneously (change `<TargetFramework>net6.0</TargetFramework>` to `net10.0`)
2. Update all 16 NuGet packages to version 10.0.2
3. Remove 5 packages now included in framework
4. Restore dependencies for entire solution
5. Build complete solution and identify all compilation errors
6. Fix all API incompatibilities discovered during build (reference breaking changes catalog)
7. Rebuild solution to verify all fixes
8. Solution builds with 0 errors ?

**Phase 2: Test Validation** (after atomic upgrade completes)
1. Run all unit tests (BCKFreightTMS.Services.Data.Tests, BCKFreightTMS.Web.Tests)
2. Address any test failures
3. All tests pass ?

### No Intermediate States

Unlike incremental strategies, there are no:
- ? Per-project upgrade cycles
- ? Phased testing
- ? Multi-targeting intermediate states
- ? Rollback after each project

The entire solution transitions from net6.0 to net10.0 in one pass.

### Dependency Ordering for All-At-Once

Even though all projects update simultaneously, understanding dependency hierarchy helps target code review efforts:

**Foundation (no deps)**:
- BCKFreightTMS.Common
- BCKFreightTMS.Data.Common  
- BCKFreightTMS.Services.Mapping
- BCKFreightTMS.Services.Messaging

**Level 1**:
- BCKFreightTMS.Data.Models (? Foundation)
- BCKFreightTMS.Web.ViewModels (? Foundation)

**Level 2-4**:
- BCKFreightTMS.Data, BCKFreightTMS.Services, BCKFreightTMS.Services.Data (increasing complexity)

**Top Level (applications)**:
- BCKFreightTMS.Web (main application)
- BCKFreightTMS.Web.Tests (tests Web)
- Sandbox (utilities)
- BCKFreightTMS.Services.Data.Tests (tests Services.Data)

**Code Review Priority**: Focus deepest review on Web/Services layers where API changes concentrate.

---

## Detailed Dependency Analysis

### Dependency Hierarchy

The solution has a clear linear dependency structure with 7 levels from foundation to top-level applications:

```
Level 0 (Foundation - 4 projects, 0 dependencies)
??? BCKFreightTMS.Common (6 dependants)
??? BCKFreightTMS.Data.Common (4 dependants)  
??? BCKFreightTMS.Services.Mapping (4 dependants)
??? BCKFreightTMS.Services.Messaging (3 dependants)

Level 1 (2 projects)
??? BCKFreightTMS.Data.Models (depends: Common, Data.Common)
??? BCKFreightTMS.Web.ViewModels (depends: Common, Services.Mapping, Data.Models)

Level 2
??? BCKFreightTMS.Data (depends: Levels 0-1)
??? BCKFreightTMS.Web.ViewModels (depends: Levels 0-1)

Level 3
??? BCKFreightTMS.Services (depends: Levels 0-2)

Level 4
??? BCKFreightTMS.Services.Data (depends: Levels 0-3, 8 total deps)

Level 5 (Top-level applications)
??? BCKFreightTMS.Web (depends: Levels 0-4, 7 direct deps)
??? BCKFreightTMS.Services.Data.Tests (depends: Levels 0-4)
??? Sandbox (depends: Levels 0-4, 8 direct deps)

Level 6 (Test applications)
??? BCKFreightTMS.Web.Tests (depends: BCKFreightTMS.Web)
```

### No Circular Dependencies

? Dependency graph is acyclic - safe for atomic upgrade without partial compilation risks.

### Projects by Layer

#### Foundation Libraries (Level 0 - 4 projects)
**Upgrade Complexity**: Minimal  
**API Changes**: None expected  
**Package Updates**: Mostly metadata packages  

- `BCKFreightTMS.Common`: Constants, enums, shared utilities
- `BCKFreightTMS.Data.Common`: Database configuration, base classes
- `BCKFreightTMS.Services.Mapping`: AutoMapper profiles
- `BCKFreightTMS.Services.Messaging`: Email/notification services

#### Data Access Layer (Levels 1-2 - 2 projects)
**Upgrade Complexity**: Low to Medium  
**API Changes**: Entity Framework Core compatible  
**Package Updates**: Entity Framework Core 6.0.0 ? 10.0.2  

- `BCKFreightTMS.Data.Models`: Entity definitions, Identity integration
- `BCKFreightTMS.Data`: DbContext, migrations, repositories

#### Business Logic Layer (Levels 3-4 - 2 projects)
**Upgrade Complexity**: Medium to High  
**API Changes**: System.Drawing (44 issues in Services), behavioral changes  
**Package Updates**: Extension packages 6.0.0 ? 10.0.2  

- `BCKFreightTMS.Services`: PDF/image generation, external services
- `BCKFreightTMS.Services.Data`: Business rules, calculations

#### Presentation Layer (Levels 5-6 - 3 applications)
**Upgrade Complexity**: Medium to High  
**API Changes**: Hosting model, Identity, middleware configuration  
**Package Updates**: ASP.NET Core packages 6.0.0 ? 10.0.2  

- `BCKFreightTMS.Web`: Main Razor Pages application (largest codebase)
- `BCKFreightTMS.Web.Tests`: Integration tests using MvcTesting
- `Sandbox`: Utility/testing application

### Dependency Impact Summary

| Project | Dependants | Dependencies | Complexity | Focus Area |
|---------|-----------|--------------|-----------|-----------|
| Common | 6 | 0 | Low | Verify exports |
| Data.Common | 4 | 0 | Low | Schema changes |
| Services.Mapping | 4 | 0 | Low | AutoMapper compat |
| Services.Messaging | 3 | 0 | Low | SendGrid compat |
| Data.Models | 5 | 2 | Low | Entity changes |
| Web.ViewModels | 3 | 3 | Low | ViewModel serialization |
| Data | 4 | 3 | Medium | EF Core migration |
| Services | 3 | 2 | **High** | **System.Drawing** |
| Services.Data | 3 | 8 | Medium | Integration |
| Web | 1 | 7 | **High** | **Hosting/Identity** |
| Web.Tests | 0 | 1 | Medium | MVC Testing |
| Services.Data.Tests | 0 | 2 | Low | EF Testing |
| Sandbox | 0 | 8 | Low | Console app |

**High-Complexity Focuses**: Services (graphics), Web (hosting/middleware/identity)

---

## Package Update Reference

### Package Status Summary

| Category | Count | Action |
|----------|-------|--------|
| **? Compatible** | 23 | Keep current versions - no update needed |
| **?? Update Required** | 16 | Upgrade to version 10.0.2 |
| **??? Remove (framework included)** | 5 | Remove package references |
| **Total** | 39 | |

### Packages Requiring Updates (16 total)

All update to version **10.0.2** for .NET 10.0 compatibility:

#### ASP.NET Core & Identity (8 packages - 11 projects affected)
```
Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore: 6.0.0 ? 10.0.2
  Projects: BCKFreightTMS.Web

Microsoft.AspNetCore.Identity.EntityFrameworkCore: 6.0.0 ? 10.0.2
  Projects: BCKFreightTMS.Data, BCKFreightTMS.Data.Models, BCKFreightTMS.Web

Microsoft.AspNetCore.Identity.UI: 6.0.0 ? 10.0.2
  Projects: BCKFreightTMS.Web, Sandbox

Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation: 6.0.0 ? 10.0.2
  Projects: Common, Data.Common, Data, Data.Models, Services, Services.Data, 
            Services.Data.Tests, Services.Mapping, Services.Messaging, Web,
            Web.Tests, Web.ViewModels, Sandbox (12 projects - all projects!)

Microsoft.AspNetCore.Mvc.Testing: 6.0.0 ? 10.0.2
  Projects: BCKFreightTMS.Web.Tests

Microsoft.VisualStudio.Web.CodeGeneration.Design: 6.0.0 ? 10.0.2
  Projects: BCKFreightTMS.Web
```

#### Entity Framework Core (7 packages - 5 projects affected)
```
Microsoft.EntityFrameworkCore.Design: 6.0.0 ? 10.0.2
  Projects: BCKFreightTMS.Data, BCKFreightTMS.Data.Models, BCKFreightTMS.Services.Data.Tests

Microsoft.EntityFrameworkCore.InMemory: 6.0.0 ? 10.0.2
  Projects: BCKFreightTMS.Services.Data.Tests

Microsoft.EntityFrameworkCore.Proxies: 6.0.0 ? 10.0.2
  Projects: BCKFreightTMS.Web

Microsoft.EntityFrameworkCore.SqlServer: 6.0.0 ? 10.0.2
  Projects: BCKFreightTMS.Data, BCKFreightTMS.Web

Microsoft.EntityFrameworkCore.Tools: 6.0.0 ? 10.0.2
  Projects: BCKFreightTMS.Data, BCKFreightTMS.Web
```

#### Microsoft.Extensions (6 packages - Sandbox only)
```
Microsoft.Extensions.Configuration: 6.0.0 ? 10.0.2
Microsoft.Extensions.Configuration.EnvironmentVariables: 6.0.0 ? 10.0.2
Microsoft.Extensions.Configuration.Json: 6.0.0 ? 10.0.2
Microsoft.Extensions.Configuration.UserSecrets: 6.0.0 ? 10.0.2
Microsoft.Extensions.Logging.Console: 6.0.0 ? 10.0.2
  Projects: Sandbox (configuration/console applications)
```

### Packages to Remove (5 total)

These packages have functionality now included in the .NET 10.0 framework:

```
Microsoft.AspNetCore.Http: 2.3.9
  Projects: Services, Services.Data, Services.Data.Tests, Web, Web.Tests, Web.ViewModels, Sandbox
  Action: Remove - functionality integrated in framework

Microsoft.AspNetCore.Http.Abstractions: 2.3.9
  Projects: Data, Services, Services.Data, Services.Data.Tests, Web, Web.Tests, Web.ViewModels, Sandbox
  Action: Remove - functionality integrated in framework

System.ComponentModel.Annotations: 5.0.0.0
  Projects: Data.Common
  Action: Remove - functionality integrated in framework

Total removal impact: 8 projects, but these are infrastructure packages - removal is automatic
```

### Compatible Packages (23 packages - Keep As-Is)

These packages are compatible with .NET 10.0 and require no updates:

```
? AngleSharp (0.16.1)
? AspNetCoreHero.ToastNotification (1.1.0)
? AutoMapper (10.1.1) - 2 projects
? BuildBundlerMinifier (3.2.449)
? CommandLineParser (2.8.0)
? Microsoft.AspNetCore.Mvc.Razor (2.3.0)
? Microsoft.AspNetCore.Mvc.ViewFeatures (2.3.0)
? Microsoft.NET.Test.Sdk (17.0.0)
? Microsoft.Web.LibraryManager.Build (2.1.113)
? Moq (4.16.1)
? Select.HtmlToPdf.NetCore (20.2.0)
? Selenium.Support (4.0.0-alpha07)
? Selenium.WebDriver (4.0.0-alpha07)
? Selenium.WebDriver.ChromeDriver (89.0.4389.2300)
? SendGrid (9.25.0)
? StyleCop.Analyzers (1.2.0-beta.205) - 13 projects
? System.Linq.Dynamic.Core (1.7.1)
? Tesseract (4.1.1)
? xunit (2.4.1)
? xunit.runner.visualstudio (2.4.3)
```

### Update Execution Strategy

**All-At-Once**: All 16 package updates applied simultaneously across all projects in single edit operation.

**No Phased Updates**: Unlike incremental strategies, all packages are updated in one pass with the framework update.

---

## Breaking Changes Catalog

### Overview

The upgrade from .NET 6.0 to .NET 10.0 introduces 61 source incompatibilities and 13 behavioral changes that require attention. These fall into three main categories:

| Category | Count | Severity | Mitigation |
|----------|-------|----------|-----------|
| **System.Drawing APIs** | 44 | Medium | NuGet package or alternatives |
| **ASP.NET Core/Hosting** | 12 | Medium | Configuration pattern updates |
| **Other API Changes** | 5 | Low | Straightforward replacements |

### Category 1: System.Drawing (44 issues - Most Critical)

**Technology**: GDI+ / System.Drawing for graphics, imaging, PDF conversion  
**Affected Projects**: 
- BCKFreightTMS.Services (39 issues - 97.5% of Services complexity)
- BCKFreightTMS.Web (5 issues)

**Issues Identified**:
```
• System.Drawing.Bitmap (11 occurrences) - Source incompatible
• System.Drawing.Image (8 occurrences) - Source incompatible  
• System.Drawing.Imaging.ImageCodecInfo (3 occurrences)
• System.Drawing.Imaging.Encoder, EncoderParameter, EncoderParameters (various)
• System.Drawing.Imaging.PixelFormat (2 occurrences)
• Image.Save(), Image.GetThumbnailImage(), Bitmap.SetResolution() methods
```

**Status in .NET 10.0**: System.Drawing is available via NuGet package `System.Drawing.Common` but:
- ? Not recommended for server scenarios
- ? Requires Windows-specific runtime for some operations
- ? Available for compatibility (project already uses it)
- ?? Consider cross-platform alternatives for new code

**Mitigation Approach - Two Options**:

**Option A: Keep System.Drawing (Lower Risk, Near-Term)**
1. Keep existing code as-is
2. System.Drawing.Common NuGet package will be referenced
3. All current functionality continues to work
4. Good for post-upgrade stabilization

**Option B: Migrate to Cross-Platform Alternative (Higher Risk, Strategic)**
1. Migrate existing System.Drawing code to SkiaSharp or ImageSharp
2. Requires additional development/testing time
3. Better long-term portability
4. Recommended for post-upgrade refactoring phase

**Recommendation**: Use **Option A** during upgrade (zero code changes), schedule **Option B** as separate refactoring task.

**Code Areas to Review**:
- `BCKFreightTMS.Services` - PDF generation, image processing
- `BCKFreightTMS.Web` - Thumbnail generation, image operations

### Category 2: ASP.NET Core API Changes (12 issues)

#### 2.1 Web Hosting Model Changes

**Issue**: Microsoft.AspNetCore.Hosting APIs deprecated
```
Affected Types:
• Microsoft.AspNetCore.Hosting.WebHostBuilder - DEPRECATED
• Microsoft.AspNetCore.WebHost - DEPRECATED  
• Microsoft.AspNetCore.Hosting.IWebHost - DEPRECATED
```

**Where It Matters**: Program.cs startup configuration

**.NET 6.0 Pattern (Old)**:
```csharp
public static void Main(string[] args)
{
    WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .Build()
        .Run();
}
```

**.NET 10.0 Pattern (New - Minimal Hosting)**:
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUserService, UserService>();
var app = builder.Build();
app.UseRouting();
app.Run();
```

**Project Impact**: Likely affects `BCKFreightTMS.Web` Startup.cs / Program.cs

**Effort**: Medium - requires understanding of Startup configuration

#### 2.2 Identity Configuration Changes

**Issue**: Identity setup methods changed signatures
```
Affected Methods:
• AddDefaultIdentity<TUser>() - signature changed
• AddEntityFrameworkStores<TDbContext>() - signature changed
```

**Where It Matters**: Startup.cs or Program.cs Identity registration

**Projects Affected**: BCKFreightTMS.Data, BCKFreightTMS.Data.Models, BCKFreightTMS.Web

**Code Pattern Change**:
```csharp
// .NET 6.0
services.AddDefaultIdentity<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// .NET 10.0
services.AddDefaultIdentity<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>(); // Same syntax usually works
// But may need explicit options configuration
```

#### 2.3 Exception Handler Middleware

**Issue**: UseExceptionHandler overload changed
```
Affected Method:
• IApplicationBuilder.UseExceptionHandler(string pathOrUrl)
```

**Projects Affected**: BCKFreightTMS.Web (if custom error pages configured)

**Pattern Change**: UseExceptionHandler now uses path-based routing instead of fallback paths

### Category 3: Behavioral Changes (13 issues)

These don't require code changes but may need testing/verification:

#### 3.1 System.Uri Constructor
```csharp
// Behavior change: additional validation in .NET 10.0
var uri = new Uri("someString");  
// May throw different exceptions or validate differently
```
**Action**: Review URI creation code; add tests if uncertain

#### 3.2 System.Net.Http.HttpContent
```csharp
// HttpContent serialization behavior changes
var content = new StringContent(data);
// May serialize differently to wire format
```
**Action**: Test HTTP interactions, especially with external APIs

#### 3.3 System.Xml.Serialization.XmlSerializer
```csharp
// XML serialization behavior changes
var serializer = new XmlSerializer(typeof(MyClass));
// May produce different XML structure or handle nulls differently
```
**Action**: Test XML integration points

#### 3.4 System.Text.Json.JsonDocument  
```csharp
// JSON parsing behavior changes
var doc = JsonDocument.Parse(json);
// May handle edge cases differently
```
**Action**: Test JSON parsing with edge cases

### Comprehensive Checklist for Code Review

**System.Drawing Areas**:
- [ ] Search Services project for `System.Drawing` usages
- [ ] Identify PDF generation code
- [ ] Review image processing code
- [ ] Check image codec operations
- [ ] Verify image save/load operations

**ASP.NET Core Areas**:
- [ ] Review Program.cs / Startup.cs
- [ ] Check Web Hosting configuration
- [ ] Verify Identity setup code
- [ ] Review Exception handling middleware
- [ ] Check custom error pages

**Behavioral Change Areas**:
- [ ] Test URI parsing with various inputs
- [ ] Test HTTP client interactions
- [ ] Verify XML serialization outputs
- [ ] Test JSON parsing edge cases
- [ ] Run full test suite for runtime behavior verification

---

## Project-by-Project Migration Plans

### Foundation Libraries (Levels 0-1)

#### BCKFreightTMS.Common
- **Current**: net6.0 | **Target**: net10.0
- **Type**: ClassLibrary | **LOC**: 397 | **Files**: 21
- **Complexity**: ?? Low | **Issues**: 2 (1 mandatory)
- **Dependencies**: None | **Dependants**: 6
- **Migration Steps**:
  1. Update `<TargetFramework>net6.0</TargetFramework>` ? `net10.0`
  2. Update NuGet: `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation` (6.0.0 ? 10.0.2)
  3. Build and verify
- **Expected Changes**: None - pure data/constants library
- **Validation**: Builds successfully

#### BCKFreightTMS.Data.Common
- **Current**: net6.0 | **Target**: net10.0
- **Type**: ClassLibrary | **LOC**: 119 | **Files**: 9
- **Complexity**: ?? Low | **Issues**: 3 (2 mandatory)
- **Dependencies**: None | **Dependants**: 4
- **Migration Steps**:
  1. Update `<TargetFramework>` to net10.0
  2. Update package: `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation` (6.0.0 ? 10.0.2)
  3. Remove `System.ComponentModel.Annotations` (now in framework)
  4. Build
- **Expected Changes**: None
- **Validation**: Builds successfully

#### BCKFreightTMS.Services.Mapping
- **Current**: net6.0 | **Target**: net10.0
- **Type**: ClassLibrary | **LOC**: 197 | **Files**: 7
- **Complexity**: ?? Low | **Issues**: 2 (1 mandatory)
- **Dependencies**: None | **Dependants**: 4
- **Migration Steps**:
  1. Update `<TargetFramework>` to net10.0
  2. Update package: `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation` (6.0.0 ? 10.0.2)
  3. AutoMapper 10.1.1 is compatible - no update needed
  4. Build
- **Expected Changes**: None - AutoMapper compatible
- **Validation**: Builds successfully

#### BCKFreightTMS.Services.Messaging
- **Current**: net6.0 | **Target**: net10.0
- **Type**: ClassLibrary | **LOC**: 106 | **Files**: 5
- **Complexity**: ?? Low | **Issues**: 3 (1 mandatory, 1 behavioral)
- **Dependencies**: None | **Dependants**: 3
- **Migration Steps**:
  1. Update `<TargetFramework>` to net10.0
  2. Update package: `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation` (6.0.0 ? 10.0.2)
  3. SendGrid (9.25.0) is compatible - no update needed
  4. Build and verify
- **Expected Changes**: None - SendGrid compatible
- **Behavioral Change**: May need testing for email sending, but code unchanged
- **Validation**: Builds successfully, test email functionality

---

### Data Access Layer (Levels 2-3)

#### BCKFreightTMS.Data.Models
- **Current**: net6.0 | **Target**: net10.0
- **Type**: ClassLibrary | **LOC**: 1238 | **Files**: 35
- **Complexity**: ?? Low | **Issues**: 4 (1 mandatory)
- **Dependencies**: Common, Data.Common | **Dependants**: 5
- **Migration Steps**:
  1. Update `<TargetFramework>` to net10.0
  2. Update packages:
     - `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (6.0.0 ? 10.0.2)
     - `Microsoft.EntityFrameworkCore.Design` (6.0.0 ? 10.0.2)
     - `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation` (6.0.0 ? 10.0.2)
  3. Build
- **Expected Changes**: None - entity definitions unchanged
- **Validation**: Builds successfully, entities load in DbContext

#### BCKFreightTMS.Data
- **Current**: net6.0 | **Target**: net10.0
- **Type**: ClassLibrary | **LOC**: 39,793 | **Files**: 62 (largest non-web project)
- **Complexity**: ?? Medium | **Issues**: 7 (1 mandatory)
- **Dependencies**: Common, Data.Common, Data.Models | **Dependants**: 4
- **Migration Steps**:
  1. Update `<TargetFramework>` to net10.0
  2. Update packages (Entity Framework Core stack):
     - `Microsoft.EntityFrameworkCore.SqlServer` (6.0.0 ? 10.0.2)
     - `Microsoft.EntityFrameworkCore.Tools` (6.0.0 ? 10.0.2)
     - `Microsoft.EntityFrameworkCore.Design` (6.0.0 ? 10.0.2)
     - `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (6.0.0 ? 10.0.2)
     - `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation` (6.0.0 ? 10.0.2)
     - `Microsoft.Extensions.Configuration.UserSecrets` (6.0.0 ? 10.0.2)
  3. Remove: `Microsoft.AspNetCore.Http.Abstractions` (now in framework)
  4. Build and review migrations
- **Expected Changes**: Check EF migrations - may need regeneration
- **Key Files**: ApplicationDbContext.cs, Migrations/*, Seeding/*
- **Validation**: Builds successfully, migrations work

---

### Business Logic Layer (Levels 4-5)

#### BCKFreightTMS.Services ?? **HIGH PRIORITY**
- **Current**: net6.0 | **Target**: net10.0
- **Type**: ClassLibrary | **LOC**: 603 | **Files**: 10
- **Complexity**: ?? High | **Issues**: 42 (1 mandatory, 39 System.Drawing)
- **Dependencies**: Common, Web.ViewModels | **Dependants**: 3
- **?? CRITICAL**: 97.5% of issues are System.Drawing API incompatibilities

**Migration Steps**:
  1. Update `<TargetFramework>` to net10.0
  2. Update packages:
     - `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation` (6.0.0 ? 10.0.2)
     - All framework dependencies
  3. **System.Drawing Analysis**:
     - Search for `System.Drawing` namespace usage
     - Identify PDF generation code
     - Identify image processing code
     - Document current functionality
  4. **System.Drawing Migration Options**:
     - ? **Option A (Recommended for this upgrade)**: Add `System.Drawing.Common` NuGet package (6.0.0)
       - Maintains compatibility
       - Zero code changes
       - Works with .NET 10.0
     - ?? **Option B (Post-upgrade refactoring)**: Migrate to SkiaSharp or ImageSharp
       - Requires code refactoring
       - Better cross-platform compatibility
       - Should be separate task
  5. Add NuGet reference: `<PackageReference Include="System.Drawing.Common" Version="6.0.0" />`
  6. Build and test PDF/image generation

**Key Files to Review**:
  - Search for `Bitmap`, `Image`, `ImageCodecInfo` usage
  - Review PDF generation code
  - Check image manipulation code

**Behavioral Changes**: 
  - System.Uri constructor (verify URI parsing if used)
  - Validate any network operations

**Testing**: Full testing of image/PDF generation required

**Validation**: 
  - ? Builds successfully
  - ? PDF generation works
  - ? Image operations function correctly

---

#### BCKFreightTMS.Services.Data
- **Current**: net6.0 | **Target**: net10.0
- **Type**: ClassLibrary | **LOC**: 2510 | **Files**: 16
- **Complexity**: ?? Medium | **Issues**: 3 (1 mandatory, 1 behavioral)
- **Dependencies**: 8 (complex hub project) | **Dependants**: 3
- **Migration Steps**:
  1. Update `<TargetFramework>` to net10.0
  2. Update packages (EF Core, ASP.NET Core):
     - `Microsoft.EntityFrameworkCore.Design` (6.0.0 ? 10.0.2)
     - `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation` (6.0.0 ? 10.0.2)
     - Extensions packages (6.0.0 ? 10.0.2)
  3. Remove: `Microsoft.AspNetCore.Http.Abstractions` (now in framework)
  4. Build
  5. Test business logic
- **Expected Changes**: Behavioral change testing for URI/Http operations
- **Validation**: Builds successfully, business logic tests pass

---

### Presentation Layer (Levels 5-6)

#### BCKFreightTMS.Web ?? **HIGH PRIORITY - MAIN APPLICATION**
- **Current**: net6.0 | **Target**: net10.0
- **Type**: AspNetCore (Razor Pages) | **LOC**: 14,504 | **Files**: 2,044
- **Complexity**: ?? High | **Issues**: 26 (3 mandatory, 5 System.Drawing)
- **Dependencies**: 7 | **Dependants**: 1 (BCKFreightTMS.Web.Tests)

**?? CRITICAL AREAS**:
1. Hosting model changes (Program.cs / Startup.cs)
2. Identity configuration updates
3. Middleware configuration
4. System.Drawing usage in image operations
5. Razor Pages compatibility

**Migration Steps**:
  1. Update `<TargetFramework>` to net10.0
  2. Update packages (complete ASP.NET Core stack):
     - `Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore` (6.0.0 ? 10.0.2)
     - `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (6.0.0 ? 10.0.2)
     - `Microsoft.AspNetCore.Identity.UI` (6.0.0 ? 10.0.2)
     - `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation` (6.0.0 ? 10.0.2)
     - `Microsoft.AspNetCore.Mvc.Testing` (6.0.0 ? 10.0.2)
     - `Microsoft.EntityFrameworkCore.*` packages (6.0.0 ? 10.0.2)
     - `Microsoft.VisualStudio.Web.CodeGeneration.Design` (6.0.0 ? 10.0.2)
     - All `Microsoft.Extensions.*` packages (6.0.0 ? 10.0.2)
  3. Remove: `Microsoft.AspNetCore.Http`, `Microsoft.AspNetCore.Http.Abstractions` (framework)
  4. **Review Program.cs/Startup.cs**:
     - Check WebHostBuilder usage - update to WebApplication model if needed
     - Verify Identity configuration syntax
     - Update exception handler if using deprecated overload
  5. **Review Razor Pages**:
     - Check for System.Drawing usage in image operations
     - Verify page models compatibility
  6. **System.Drawing**: Add package reference if needed (same as Services project)
  7. Build and run

**Key Files to Review**:
  - `Program.cs` / `Startup.cs` - hosting model
  - `ApplicationDbContext.cs` - Entity Framework setup
  - `Infrastructure/` - middleware, extensions
  - `Controllers/` - ASP.NET Core controller changes
  - Views with image generation

**Behavioral Changes**:
  - Uri constructor
  - HttpContent serialization
  - Exception handler middleware

**Testing**: 
  - Full application startup
  - Identity authentication
  - Image/PDF operations
  - External API calls

**Validation**:
  - ? Application starts without errors
  - ? Identity authentication works
  - ? Database migrations apply
  - ? Pages render correctly
  - ? File uploads/image operations work

---

#### BCKFreightTMS.Web.ViewModels
- **Current**: net6.0 | **Target**: net10.0
- **Type**: ClassLibrary | **LOC**: 2233 | **Files**: 73
- **Complexity**: ?? Low | **Issues**: 2 (1 mandatory)
- **Dependencies**: Common, Services.Mapping, Data.Models | **Dependants**: 3
- **Migration Steps**:
  1. Update `<TargetFramework>` to net10.0
  2. Update package: `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation` (6.0.0 ? 10.0.2)
  3. Verify AutoMapper compatibility (10.1.1 - compatible)
  4. Build
- **Expected Changes**: None - ViewModels are POCOs
- **Validation**: Builds successfully

---

#### BCKFreightTMS.Web.Tests
- **Current**: net6.0 | **Target**: net10.0
- **Type**: AspNetCore (Test) | **LOC**: 106 | **Files**: 6
- **Complexity**: ?? Medium | **Issues**: 18 (3 mandatory, 6 source incompatible, 7 behavioral)
- **Dependencies**: BCKFreightTMS.Web | **Dependants**: None
- **Migration Steps**:
  1. Update `<TargetFramework>` to net10.0
  2. Update packages:
     - `Microsoft.AspNetCore.Mvc.Testing` (6.0.0 ? 10.0.2)
     - All supporting packages
  3. Update test patterns if WebApplicationFactory setup changed
  4. Verify Selenium compatibility (4.0.0-alpha07 - check for updates)
  5. Build and run all tests
- **Expected Changes**: WebApplicationFactory initialization, Selenium usage
- **Testing**: Full integration test suite execution
- **Validation**: All tests pass

---

#### Sandbox (Utility/Test Application)
- **Current**: net6.0 | **Target**: net10.0
- **Type**: DotNetCoreApp | **LOC**: 97 | **Files**: 3
- **Complexity**: ?? Low | **Issues**: 11 (1 mandatory, 4 source incompatible)
- **Dependencies**: 8 (tools/testing project) | **Dependants**: None
- **Migration Steps**:
  1. Update `<TargetFramework>` to net10.0
  2. Update packages:
     - All `Microsoft.Extensions.*` packages (6.0.0 ? 10.0.2)
     - `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation` (6.0.0 ? 10.0.2)
  3. Review command-line parsing if used
  4. Build
- **Expected Changes**: Configuration setup if modified
- **Validation**: Builds successfully, command-line execution works

---

#### BCKFreightTMS.Services.Data.Tests
- **Current**: net6.0 | **Target**: net10.0
- **Type**: DotNetCoreApp (xUnit Tests) | **LOC**: 568 | **Files**: 7
- **Complexity**: ?? Low | **Issues**: 4 (1 mandatory)
- **Dependencies**: Services.Data, Data | **Dependants**: None
- **Migration Steps**:
  1. Update `<TargetFramework>` to net10.0
  2. Update packages:
     - `Microsoft.EntityFrameworkCore.InMemory` (6.0.0 ? 10.0.2)
     - `Microsoft.EntityFrameworkCore.Design` (6.0.0 ? 10.0.2)
     - All supporting packages
  3. Verify xUnit compatibility (2.4.1 - compatible)
  4. Build and run tests
- **Expected Changes**: None - xUnit compatible
- **Validation**: All unit tests pass

---

## Risk Management

### Risk Assessment Matrix

| Risk Area | Severity | Likelihood | Impact | Mitigation |
|-----------|----------|-----------|--------|-----------|
| **System.Drawing APIs** | Medium | High | 44 code locations | Add NuGet package; keep existing code |
| **Web Hosting Model** | Medium | High | Application startup | Update Program.cs pattern |
| **Identity Configuration** | Medium | Medium | Authentication flow | Update setup code, test login |
| **EF Core Migrations** | Low | Low | Database initialization | Run migrations, verify DB state |
| **Test Failures** | Low | Medium | Application validation | Run full test suite, debug failures |
| **Third-party Packages** | Low | Low | Compatibility issues | Verify compatible versions pre-upgrade |

### Mitigation Strategies

#### System.Drawing (44 issues)
- **Prevention**: Review System.Drawing usage before upgrade
- **During Upgrade**: Add System.Drawing.Common NuGet package to project files
- **Testing**: Test all image/PDF operations after upgrade
- **Post-Upgrade**: Schedule refactoring to SkiaSharp or ImageSharp for long-term

#### Web Hosting (Program.cs/Startup.cs)
- **Prevention**: Understand current hosting configuration before upgrade
- **During Upgrade**: Update to WebApplication model if using WebHostBuilder
- **Testing**: Test application startup and middleware pipeline
- **Post-Upgrade**: Verify all startup logging and configuration works

#### Identity Configuration
- **Prevention**: Document current Identity setup
- **During Upgrade**: Test login/authentication flows
- **Testing**: Full authentication scenarios (login, logout, role-based access)
- **Post-Upgrade**: Verify user profile and permission management

#### EF Core Migrations
- **Prevention**: Backup database before upgrade
- **During Upgrade**: EF migrations should apply automatically with tool update
- **Testing**: Verify database schema matches expectations
- **Post-Upgrade**: Test data access operations, stored procedures if used

### Rollback Plan

If critical issues prevent deployment:

1. **Immediate Rollback**: Switch back to upgrade-to-NET10 ? main branch (no commits yet)
   ```bash
   git checkout main
   ```

2. **Analysis Phase**:
   - Document specific error
   - Review breaking changes catalog
   - Identify blocker

3. **Targeted Fix**:
   - Fix specific issue in isolated branch
   - Re-test
   - Redeploy

4. **Full Restart** (if necessary):
   - Delete upgrade-to-NET10 branch
   - Create new upgrade branch
   - Re-attempt with fix applied

### High-Risk Projects Special Attention

#### BCKFreightTMS.Services (System.Drawing)
- **Detailed Review Required**: Yes
- **Test Coverage Needed**: Comprehensive image/PDF testing
- **Contingency**: Keep System.Drawing.Common on current version if issues arise

#### BCKFreightTMS.Web (Hosting/Identity/Main App)
- **Detailed Review Required**: Yes
- **Test Coverage Needed**: Full integration testing (startup, auth, pages)
- **Contingency**: Can roll back to working state before identity tests

---

## Testing & Validation Strategy

### Multi-Level Testing Approach

#### Level 1: Build Validation (Immediate)
After all code changes:
```bash
dotnet build --configuration Release
# Expected: 0 errors, 0 warnings
```

**Validation Checks**:
- ? All 13 projects compile
- ? No missing dependencies
- ? No obsolete API usage remains
- ? No version conflicts

**Success Criteria**: Green build, all projects compile

#### Level 2: Unit Tests (Per Project)
```bash
dotnet test BCKFreightTMS.Services.Data.Tests
dotnet test BCKFreightTMS.Web.Tests
```

**Test Projects**:
1. `BCKFreightTMS.Services.Data.Tests` - Business logic validation
   - Data access patterns
   - Service calculations
   - EF Core interactions

2. `BCKFreightTMS.Web.Tests` - Web application validation
   - Integration tests
   - Page rendering
   - Authentication flows
   - API endpoints (if any)

**Success Criteria**: All unit tests pass

#### Level 3: Application Integration Testing

**Manual Testing Scenarios**:

1. **Application Startup**
   - [ ] Application starts without errors
   - [ ] Logging initializes correctly
   - [ ] Configuration loads properly
   - [ ] Database connections work

2. **Authentication & Identity**
   - [ ] User can login
   - [ ] User can logout
   - [ ] Role-based access works
   - [ ] User profiles load correctly
   - [ ] Password reset works (if enabled)

3. **Core Features** (data dependent)
   - [ ] Dashboard/home page loads
   - [ ] Data retrieval works (tables, searches)
   - [ ] Data creation works (forms)
   - [ ] Data updates work (edits)
   - [ ] Data deletion works

4. **Image/PDF Operations** (System.Drawing)
   - [ ] Thumbnail generation works
   - [ ] Image upload/storage works
   - [ ] PDF generation works (if applicable)
   - [ ] File downloads work

5. **External Integrations** (if any)
   - [ ] Email sending works
   - [ ] External API calls work
   - [ ] Web service integrations work

6. **Performance**
   - [ ] Page load times acceptable
   - [ ] No database timeout issues
   - [ ] No memory leaks
   - [ ] Reasonable CPU usage

#### Level 4: Regression Testing

Run against existing test data:
- [ ] Known workflows function correctly
- [ ] Edge cases handled properly
- [ ] Error messages remain helpful
- [ ] Validation rules unchanged

### Specific Test Cases for Breaking Changes

#### System.Drawing Tests
```
Test: Image Upload and Thumbnail Generation
- Create test image file
- Upload to system
- Verify thumbnail created
- Verify image displays correctly
- Verify dimensions/format preserved

Test: PDF Generation (if used)
- Generate sample PDF
- Verify file format
- Verify content accuracy
- Verify encoding
```

#### ASP.NET Core Tests
```
Test: Application Startup
- Verify Startup/Program.cs configuration
- Verify middleware pipeline
- Verify dependency injection
- Verify logging

Test: Exception Handling
- Trigger error condition
- Verify error page displays
- Verify logging captures exception
- Verify user-friendly message shown
```

#### Identity Tests
```
Test: Login Flow
- Credentials invalid ? error
- Credentials valid ? authenticated
- Tokens generated correctly
- Session management works

Test: Role-based Access
- Unauthorized access ? denied
- Authorized access ? allowed
- Role changes take effect
```

### Test Execution Plan

**Phase 1: Automated Testing**
1. `dotnet build` - Full solution build
2. `dotnet test` - All unit tests
3. Report: X tests passed, Y tests failed

**Phase 2: Application Testing**
1. Start application in Debug mode
2. Execute manual test scenarios above
3. Document any issues found
4. Verify fixes work

**Phase 3: Validation**
1. Confirm all tests pass
2. Verify application functionality
3. Performance baseline acceptable
4. Ready for deployment

### Known Testing Considerations

- **Selenium Tests**: Web.Tests uses Selenium 4.0.0-alpha07 (pre-release)
  - Consider upgrading to stable Selenium 4.x for reliability
  - Test browser compatibility (Chrome, Firefox)
  - May need ChromeDriver updates

- **EF Core Tests**: Services.Data.Tests uses InMemory provider
  - Verify InMemory provider behavior unchanged
  - Run against actual SQL Server if integration issues appear
  - Check migration compatibility

---

## Complexity & Effort Assessment

### Complexity Ratings by Project

| Project | Complexity | Reasoning | Estimated LOC Review | Focus Area |
|---------|-----------|-----------|----------------------|-----------|
| Common | ?? Low | Constants, enums | Minimal | Verify exports |
| Data.Common | ?? Low | Configuration | Minimal | Base classes |
| Services.Mapping | ?? Low | AutoMapper compatible | Minimal | Profile verification |
| Services.Messaging | ?? Low | SendGrid compatible | Minimal | Email functionality |
| Data.Models | ?? Low | Entities unchanged | Minimal | Type definitions |
| Web.ViewModels | ?? Low | POCOs, no logic | Minimal | Property mapping |
| Data | ?? Medium | EF Core + Migrations | Moderate | DbContext, migrations |
| Services.Data | ?? Medium | Complex hub | Moderate | Integration points |
| Services.Data.Tests | ?? Medium | xUnit + EF testing | Moderate | Test logic |
| Web.Tests | ?? Medium | Integration tests | Moderate | Hosting, middleware |
| Sandbox | ?? Low | Console app | Minimal | Startup |
| **Services** | ?? **High** | **System.Drawing (39 issues)** | **40+ LOC** | **Image/PDF generation** |
| **Web** | ?? **High** | **Hosting/Identity (12 issues)** | **15+ LOC** | **Program.cs, Auth, Pages** |

### Effort Distribution

**Low Complexity (9 projects)**:
- Update project files
- Update NuGet packages
- Verify build
- Run unit tests
- **Typical effort per project**: Minimal review

**Medium Complexity (3 projects)**:
- Update project files
- Update NuGet packages
- Run full test suite
- Verify integration points
- **Typical effort per project**: Moderate code review

**High Complexity (2 projects)**:
- Update project files
- Update NuGet packages  
- **System.Drawing review** (Services) OR **Hosting/Identity review** (Web)
- Run full integration tests
- Manual testing scenarios
- **Typical effort per project**: Detailed code review + testing

### All-At-Once Strategy Advantages for Effort

? **Single coordinated update** - no repeated setup/teardown  
? **Unified test cycle** - one comprehensive test pass  
? **No multi-targeting** - simpler project files  
? **Clear success criteria** - all-or-nothing validation  

### Total Effort Estimate

This is **not a time estimate** (too variable), but a **complexity/effort scale**:

- **Project File Updates**: Low (systematic, 13 files)
- **Package Updates**: Low (systematic, 16 packages across files)
- **System.Drawing Review**: Medium (39 locations to verify)
- **Hosting/Identity Review**: Medium (Program.cs, startup code)
- **Build & Fix**: Medium (compile errors, breaking changes)
- **Unit Test Execution**: Low (automated)
- **Integration Testing**: Medium (manual scenarios)
- **Total Complexity**: Medium (manageable, clear scope)

**Key Success Factors**:
- Clear dependency structure
- Modern SDK-style projects
- Good test coverage
- Limited third-party breaking changes
- All-at-once simplicity

---

## Source Control Strategy

### All-At-Once Branching & Commit Approach

**Goal**: Single atomic commit for entire .NET 10.0 upgrade

#### Branch Setup
```
Current State:
??? main (production, net6.0)
??? upgrade-to-NET10 (upgrade work, created)
```

**Branch Policy**: 
- All changes made on `upgrade-to-NET10`
- Single commit when all changes complete
- Merge to main via Pull Request

#### Commit Strategy: Single Unified Commit

**Rationale**: 
- All-at-once upgrade = single atomic operation
- Easy to review ("upgrade to .NET 10.0")
- Easy to revert if needed (one commit to undo)
- Clear in git history

**Commit Message**:
```
upgrade: migrate solution to .NET 10.0

- Update all 13 projects to target net10.0
- Update 16 NuGet packages to version 10.0.2
- Remove 5 packages now in framework
- Update System.Drawing references (add NuGet package)
- Update hosting model in Program.cs/Startup.cs
- Update Identity configuration for .NET 10.0
- Verify all tests pass
- Solution builds with 0 errors

Fixes #XXX (if applicable)
```

#### Detailed Commit Checklist

```
Before committing, verify:
? All 13 project files updated (TargetFramework)
? All 16 packages updated to 10.0.2
? All 5 packages removed from references
? System.Drawing.Common added to Services, Web
? Program.cs/Startup.cs updated (if needed)
? Identity configuration updated
? Solution builds: dotnet build
? All tests pass: dotnet test
? No warnings in build output
? No compilation errors
```

#### Merge to Main

**Pull Request Template**:
```markdown
## .NET 10.0 Upgrade

### Changes
- Migrated solution from .NET 6.0 to .NET 10.0
- Updated all NuGet packages
- Handled System.Drawing API changes
- Updated ASP.NET Core configuration

### Testing
- ? Full solution builds (0 errors, 0 warnings)
- ? Unit tests pass (BCKFreightTMS.Services.Data.Tests)
- ? Integration tests pass (BCKFreightTMS.Web.Tests)
- ? Manual testing: [list scenarios tested]

### Known Issues
[If any, list them here]

### Rollback Plan
If issues found post-merge:
- Revert commit: git revert [commit-sha]
- Restore from backup
- Investigate issue on separate branch
```

#### Post-Merge Workflow

1. **Tag Release** (once deployed):
   ```bash
   git tag -a v2.0.0-net10 -m "Release: .NET 10.0 migration"
   ```

2. **Update Documentation**:
   - Update README with .NET 10.0 requirement
   - Update CI/CD configuration if needed
   - Document deployment steps

3. **Close Related Issues**:
   - Link PRs to issues
   - Document migration notes

### Reverting if Needed

**Quick Revert** (before merge to main):
```bash
git reset --hard origin/upgrade-to-NET10  # Go back to last known state
```

**Revert Merged Commit** (after merge):
```bash
git revert [commit-sha]  # Creates inverse commit
git push origin main
```

### Communication Plan

**Before Upgrade**:
- Notify team of planned upgrade
- Schedule upgrade work
- Prepare test environment

**During Upgrade**:
- Document blockers in real-time
- Keep team informed of progress
- Test as you go

**After Upgrade**:
- Communicate successful migration
- Document lessons learned
- Share migration notes with team

---

## Success Criteria

### Technical Success Criteria

#### Target Framework Updates
- ? All 13 projects have `<TargetFramework>net10.0</TargetFramework>`
- ? No projects remain on net6.0
- ? Project files validated with `dotnet list package`

#### NuGet Package Updates
- ? 16 packages updated to version 10.0.2
- ? 5 packages successfully removed (no compilation errors)
- ? 23 compatible packages remain unchanged
- ? No package conflicts or dependency issues
- ? `dotnet restore` completes without errors

#### Build Success
- ? `dotnet build --configuration Release` completes with 0 errors
- ? No warnings related to framework compatibility
- ? No obsolete API usage warnings
- ? All 13 projects build successfully
- ? Solution builds as a whole (cross-project references work)

#### API Compatibility Fixes
- ? System.Drawing migrations handled (NuGet package added)
- ? Hosting model updated (Program.cs pattern)
- ? Identity configuration updated
- ? Exception handler patterns updated if needed
- ? No breaking change issues remain

#### Test Success
- ? `BCKFreightTMS.Services.Data.Tests`: All tests pass
- ? `BCKFreightTMS.Web.Tests`: All tests pass
- ? No test failures or flakiness
- ? Code coverage maintained or improved

#### Application Functionality
- ? Application starts without errors
- ? Database migrations apply successfully
- ? User authentication works (login/logout)
- ? Role-based access control works
- ? Core data operations work (CRUD)
- ? Image/PDF operations work
- ? External integrations work (email, APIs)

### Quality Criteria

#### Code Quality
- ? No compiler warnings
- ? StyleCop analyzers pass (existing rules)
- ? No obsolete APIs used
- ? Code is idiomatic for .NET 10.0

#### Performance
- ? Application startup time acceptable (no regression)
- ? Page load times acceptable (no degradation)
- ? Database query performance maintained
- ? Memory usage reasonable
- ? CPU usage normal

#### Documentation
- ? Migration notes documented
- ? Breaking changes documented
- ? Deployment steps clear
- ? README updated (.NET 10.0 requirement)

### Success Verification Checklist

Before considering upgrade complete:

**Pre-Deployment Validation**
- [ ] Solution builds clean (0 errors)
- [ ] All tests pass
- [ ] Manual integration testing complete
- [ ] System.Drawing functionality verified
- [ ] Identity/authentication tested
- [ ] External APIs tested
- [ ] Performance acceptable

**Git/Source Control**
- [ ] All changes committed on upgrade-to-NET10 branch
- [ ] Commit message clear and complete
- [ ] Pull Request created with documentation
- [ ] Code review completed
- [ ] PR approved and merged to main

**Documentation**
- [ ] Migration plan completed
- [ ] Known issues documented
- [ ] Deployment notes created
- [ ] Team notified of changes

**Post-Deployment**
- [ ] Application deployed to production (or prepared for deployment)
- [ ] Production tests pass
- [ ] No unexpected errors in logs
- [ ] Performance baseline verified
- [ ] Rollback plan documented

### Definition of "Upgrade Complete"

The upgrade is **complete and successful** when:

1. ? All 13 projects target .NET 10.0
2. ? All 16 packages updated to 10.0.2
3. ? All 5 packages removed from references
4. ? Solution builds with 0 errors and 0 warnings
5. ? All unit tests pass
6. ? All integration tests pass
7. ? Manual testing scenarios all pass
8. ? Changes committed to git
9. ? Pull request merged to main
10. ? Documentation updated

**Anything less than this is incomplete.**

### Post-Upgrade Tasks (Not Part of This Plan)

These items are **out of scope** for this migration but recommended:

- Evaluate System.Drawing alternatives (SkiaSharp, ImageSharp)
- Upgrade Selenium WebDriver from alpha to stable release
- Review and optimize Entity Framework Core usage
- Update CI/CD pipelines for .NET 10.0
- Performance testing and optimization
- Security audit for .NET 10.0 specific concerns
- Update internal documentation and training

---

## Appendix: Assessment Summary Reference

**Assessment File**: `.github/upgrades/assessment.md`
**Generated**: [During analysis phase]
**Total Issues Found**: 127 (18 mandatory, 109 potential)
**Projects Analyzed**: 13
**Files Affected**: 25 of 430 (5.8%)
**Estimated LOC Impact**: 74+ lines

**Key Metrics**:
- Total NuGet Packages: 39
- Packages with Issues: 16 updates + 5 removals = 21 total
- Compatible Packages: 23
- Source Incompatibilities: 61
- Behavioral Changes: 13
- System.Drawing Issues: 44 (79.6% of incompatibilities)

**Dependencies**:
- Maximum Depth: 6 levels (Web ? Foundation)
- Circular Dependencies: None
- Foundation Projects: 4
- Application Endpoints: 2 (Web, Sandbox)
- Test Projects: 2

---
