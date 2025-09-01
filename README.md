# TemplateDotNetCoreProject
This repo will serve as a basic template for a ASP .NET core Web API project which uses PostgreSQL db and Scalar API for API visualization. 

## Features

- ASP.NET Core 9.0 Web API
- PostgreSQL database integration via Entity Framework Core
- API versioning with [Asp.Versioning]
- Serilog for structured logging (console and file output)
- Scalar API for OpenAPI visualization
- Modular service and extension registration
- Example domain: Employees, Users, Tickets (with relationships)
- Seed data logic for development and testing
- Integration and unit test projects scaffolded
- StyleCop, Roslynator, and other analyzers for code quality

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Node.js](https://nodejs.org/) (if using Scalar UI locally)
- [EF Core CLI Tools](https://learn.microsoft.com/ef/core/cli/dotnet)

### Setup

1. **Clone the repository:**
   ```sh
   git clone
   cd TemplateDotNetCoreProject
   ```

2. **Configure the database:**
   - Update the connection string in `src/TemplateProject.API/appsettings.Development.json` and `appsettings.json` as needed.

3. **Apply database migrations:**
   Run from the `src` folder:
   ```sh
   dotnet ef migrations add InitialMigration --project TemplateProject.Services --startup-project TemplateProject.API --output-dir Database/Migrations
   dotnet ef database update --project TemplateProject.Services --startup-project TemplateProject.API
   ```

4. **Run the API:**
   ```sh
   dotnet run --project src/TemplateProject.API
   ```

5. **View API documentation:**
   - Swagger/Scalar UI will be available at `/swagger` or `/scalar` (depending on configuration).

### Project Structure

- `src/TemplateProject.API` - Main Web API project
- `src/TemplateProject.Services` - Business logic, database models, and services
- `test/TemplateProject.UnitTests` - Unit tests
- `test/TemplateProject.IntegrationTests` - Integration tests

### Code Quality

- Code style and analyzers are configured via `.editorconfig` and `Directory.Packages.props`.
- StyleCop, Roslynator, and other analyzers are enabled.
- Logging is configured with Serilog (console and file).

### Seeding Data

The API seeds initial Employees, Users, and Tickets if the database is empty on startup (see `Program.cs`).

---

## Useful Commands

- **Add migration:**
  ```sh
  dotnet ef migrations add <MigrationName> --project TemplateProject.Services --startup-project TemplateProject.API --output-dir Database/Migrations
  ```
- **Update database:**
  ```sh
  dotnet ef database update --project TemplateProject.Services --startup-project TemplateProject.API
  ```
- **Run tests:**
  ```sh
  dotnet test
  ```

---

## License

MIT License