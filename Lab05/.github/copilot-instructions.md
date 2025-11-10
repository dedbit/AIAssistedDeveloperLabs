# Employee Management System - AI Coding Agent Guide

## Architecture Overview

This is a 4-project .NET 6 solution with clean architecture pattern:
- **EmployeeManagement.API**: Web API backend using EF Core with SQL Server
- **EmployeeManagement.Core**: Domain entities, interfaces, and DTOs (no dependencies)
- **EmployeeManagement.Web**: Blazor WebAssembly frontend
- **EmployeeManagement.Tests**: Xunit tests with in-memory database

**Key Integration Point**: The Blazor app hardcodes API base address `https://localhost:53399` in `EmployeeManagement.Web/Program.cs`. Update both projects' `launchSettings.json` if ports change.

## Code Style Conventions (see code-style.md)

1. **No `var` keyword**: Always use explicit types (`Employee employee` not `var employee`)
2. **XML documentation required**: Add `<summary>` tags to all classes with example parameter values
3. **Comments mandatory**: Add explanatory comments throughout code
4. **Test naming**: Use `{methodundertest}_{scenario}` format, all lowercase (e.g., `addemployee_validdata_returnsemployeewithid`)

## Extension Methods Pattern

Service registration is centralized in extension methods under `EmployeeManagement.API/Extensions/`:
- `ServiceCollectionExtensions.AddDatabaseServices()`: Configures EF Core DbContext and repositories
- `ServiceCollectionExtensions.AddSwaggerServices()`: Sets up OpenAPI with custom title
- `ServiceCollectionExtensions.AddCorsServices()`: Defines "AllowBlazor" policy (currently allows all origins)
- `ApplicationBuilderExtensions.UseSwaggerWithUI()`: Configures Swagger UI as root path (`RoutePrefix = string.Empty`)

Always follow this pattern when adding new service configurations.

## Database Workflow

**Auto-migration at startup**: `Program.cs` applies pending migrations on app start using `dbContext.Database.Migrate()` in a scope.

**Manual migrations** (when needed):
```powershell
cd .\EmployeeManagement.API\
dotnet ef migrations add MigrationName
dotnet ef database update
```

Connection string in `appsettings.json` uses "DefaultConnection" key.

## Testing Patterns

- Tests use **in-memory database**: Each test creates isolated DbContext via `UseInMemoryDatabase(Guid.NewGuid().ToString())`
- **Repository testing**: Create `EmployeeDbContext` and `EmployeeRepository`, test repository methods directly
- **Controller testing**: Inject mocked repository, assert on ActionResult types (`OkObjectResult`, `CreatedAtActionResult`, `NotFoundResult`)
- Tests include deliberate bugs for debugging exercises (e.g., `AddAsync` returns `null` instead of employee, `DeleteAsync` throws exception)

## Known Issues (Intentional for Lab)

The `EmployeeRepository` contains bugs used for debugging training:
1. `AddAsync()` returns `null` instead of the created employee
2. `DeleteAsync()` throws `Exception("Not implemented")` before actual delete logic

Do not fix these without explicit instruction - they're teaching examples for the "Debug with Copilot" workflow.

## Project Context

This is **Lab05** in a Copilot training series. See `Instructions.md` for debugging workflow exercises. The solution demonstrates VS Code and Visual Studio debugging features with intentional test failures.
