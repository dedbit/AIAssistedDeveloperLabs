## Purpose
This file gives concise, repository-specific guidance to AI coding agents so they can be productive immediately in this codebase.

## Big picture (what this repo is)
- AzureFunctions.Api: an Azure Functions project (net6) that hosts several HTTP-triggered functions in `AzureFunctions.Api/Functions`.
- WebApi: a small ASP.NET Web API (net7) in `WebApi/` used for examples or parallel API work.
- Tests: `AzureFunctions.Api.Tests` contains xUnit tests and local mocks in `Mocks/`.

Key patterns and boundaries:
- Dependency injection for Functions is configured in `AzureFunctions.Api/Startup.cs` (inherits `FunctionsStartup`). Add or change DI bindings here.
- Data access: `BooksRepository` uses EF Core with `gravity_booksContext` (in-memory DB wired in `Startup.cs` for local/test runs). `ProjectRepository` uses Azure Table Storage via `Clients/TableStorageClient.cs`.
- Configuration: `Managers/ConfigManager.cs` reads environment variables and local JSON files (`debug.settings.json`, `secrets.settings.json`) — tests and local runs depend on these resolution paths.

## How to build, run and test (concrete)
- Build entire solution: `dotnet build AzureFunctions.Api.sln` (run from `src/` or repository root containing the .sln).
- Run unit tests: `dotnet test AzureFunctions.Api.Tests/AzureFunctions.Api.Tests.csproj` (run from `src/` so test sample file paths resolve)
- Run the Functions locally: use Azure Functions Core Tools in the `AzureFunctions.Api` folder (e.g., `func start`); ensure `AzureWebJobsStorage` is set (or provided in `debug.settings.json`) so Table/Storage clients do not throw.
- Publish: see `AzureFunctions.Api/Publish.ps1` for an existing PS-based publish script.

## Project-specific conventions and gotchas
- Config resolution: `ConfigManager.GetConfigValue("AzureWebJobsStorage")` can return from env vars OR JSON files. Tests rely on `debug.settings.json` being present in the test working directory.
- DI registrations: `Startup.cs` registers `ConfigManager` and `ProjectRepository` as singletons and `BooksRepository` as scoped. When adding services, modify `Startup.cs` rather than scattering new singletons.
- Repositories and testability: `ProjectRepository` methods are `virtual` and a `ProjectRepositoryFake` exists in `AzureFunctions.Api.Tests/Mocks` — prefer using or adding fakes to keep tests isolated from real Azure Table Storage.
- TableStorage client: `Clients/TableStorageClient.cs` uses the older `Microsoft.WindowsAzure.Storage.Table` types and synchronous patterns returning Task/Result in places. Be conservative when modifying concurrency here.
- Seeding: `ProjectRepository` calls `SeedData().Wait()` in its constructor — this is synchronous blocking in ctor (observed pattern in repo). Be careful changing this without updating callers.

## Integration points
- Azure Table Storage: used by `ProjectRepository` -> `Clients/TableStorageClient.cs`. The config key is `AzureWebJobsStorage` (see `ConfigManager`).
- EF Core InMemory: wired in `Startup.cs` for `gravity_booksContext` (enables tests and local runs without a SQL server).
- Azure Functions runtime: functions depend on the Functions host (host.json is present). Local dev uses `func` or Visual Studio.

## Concrete examples to reference
- Constructor DI in functions: `AzureFunctions.Api/Functions/GetCustomers.cs` (constructor injection of `BooksRepository`).
- Config usage: `AzureFunctions.Api/Repositories/ProjectRepository.cs` reads `AzureWebJobsStorage` via `ConfigManager`.
- Table client usage: see `AzureFunctions.Api/Clients/TableStorageClient.cs` for Insert/InsertOrMerge/Retrieve/Delete patterns.
- Tests: `AzureFunctions.Api.Tests/FunctionsTests.cs` shows usage of `HttpMock` (in `Mocks/`) and `Samples/ProjectGetRequest.json` for request bodies. Run tests from `src/` so file paths resolve.

## Guidance for AI edits
- When adding features prefer to:
  - Add or update DI bindings in `Startup.cs` (FunctionsStartup) and keep lifetimes consistent (singleton vs scoped).
  - Use existing interfaces or virtual methods for repositories so tests can inject fakes (e.g., `ProjectRepository` is virtual-friendly; `ProjectRepositoryFake` exists).
  - Avoid changing `ConfigManager` resolution order; tests rely on the current fallbacks.
- If you need to add integration tests that touch Table Storage, prefer creating a local fake `TableStorageClient` and registering it in tests (Mocks contains examples).

## Where to look first (quick navigation)
- Dependency setup: `AzureFunctions.Api/Startup.cs`
- Functions: `AzureFunctions.Api/Functions/*.cs` (GetCustomers,  HttpTrigger, GetProject)
- Repositories: `AzureFunctions.Api/Repositories/*` (BooksRepository, ProjectRepository)
- Table client: `AzureFunctions.Api/Clients/TableStorageClient.cs`
- Config: `AzureFunctions.Api/Managers/ConfigManager.cs`
- Tests and Mocks: `AzureFunctions.Api.Tests/` (look in `Mocks/` and `Samples/`)

If any section here is unclear or you'd like me to expand examples (code snippets for DI changes, adding a fake Table client, or a sample test), tell me which area and I'll iterate.
