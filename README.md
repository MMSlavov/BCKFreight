# BCKFreight Transportation Management System

A web-based Transportation Management System (TMS). This system manages freight orders, carriers, drivers, vehicles, and related logistics operations. It provides tools for order management, carrier tracking, invoice handling, and comprehensive business logic for transportation workflows.

## Technology Stack

- **Framework**: .NET 10.0
- **Web Framework**: ASP.NET Core
- **ORM**: Entity Framework Core 10.0.2
- **Database**: SQL Server (via EF Core)
- **Authentication**: ASP.NET Core Identity
- **API**: RESTful Web API
- **Code Analysis**: StyleCop, EditorConfig

## Project Structure

The solution is organized into multiple layers and functional areas:

## Key Entities & Features

### Core Entities

- **Order**: Main entity representing freight orders
- **Company**: Freight companies and business entities
- **Person**: Employees, drivers, and contacts
- **Vehicle**: Trucks, trailers, and transportation equipment
- **Cargo**: Freight items and cargo information
- **Driver Order**: Assignment of drivers to specific orders
- **Carrier Order**: Carrier-specific order information
- **Invoice**: Both incoming and outgoing invoice management

### Domain Concepts

- **Order Management**: Create, track, and manage transportation orders
- **Accounting Types**: Multiple accounting classification systems
- **VAT Handling**: Support for multiple VAT reasons and tax countries
- **Bank Movements**: Financial tracking and reconciliation
- **Action Tracking**: Record actions taken on orders with reasons and status
- **Multi-Currency Support**: Handle transactions in different currencies

### Building the Solution

```bash
cd src
dotnet build BCKFreightTMS.sln
```

### Running the Application

```bash
cd src/BCKFreightTMS.Web
dotnet run
```

The application will be available at `https://localhost:5001` by default.

## Architecture Highlights

- **Layered Architecture**: Clear separation between Data, Services, and Web layers
- **Entity Framework Core**: ORM with support for complex relationships
- **Soft Delete Pattern**: Entities use soft deletes via `BaseDeletableModel`
- **Dependency Injection**: Built-in .NET DI container
- **Service Layer Pattern**: Business logic isolated from data access
- **DTO Mapping**: AutoMapper for entity-to-DTO conversions

## Security

- ASP.NET Core Identity for user authentication and management
- Role-based access control (RBAC)
- User secrets for sensitive configuration

## License

See [LICENSE](LICENSE) file for details.

