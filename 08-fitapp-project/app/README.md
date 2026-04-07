# FitApp Reservations

FitApp Reservations is a production-leaning MVP web app for managing personal trainer reservations. It uses Razor Pages, Entity Framework Core, and SQL Server so it can be deployed to Azure App Service with Azure SQL Database.

## Project overview

The application supports:

- Trainers with active/inactive status and hourly rates
- Clients with optional email, phone, and notes
- Reservations with status tracking and schedule validation
- Health checks at `/health`
- Seed data for demos when the database is empty
- Environment-variable based configuration for Azure

## Architecture

The solution keeps the structure intentionally simple:

- `FitAppReservations/Data`: `ApplicationDbContext` and startup database initialization
- `FitAppReservations/Models`: entities, enum, and shared reservation input model
- `FitAppReservations/Pages`: Razor Pages UI for home, trainers, clients, reservations, error, and not-found pages
- `FitAppReservations/Services`: reservation business validation logic
- `FitAppReservations/Migrations`: EF Core migration history
- `FitAppReservations/wwwroot`: Bootstrap assets and site styling

Business rules are enforced on the server through `ReservationService`:

- Reservation end time must be after start time
- Trainers cannot receive overlapping reservations on the same date
- Inactive trainers cannot receive new reservations
- Email is optional, but validated if supplied
- Hourly rate must be zero or higher

## Tech stack

- ASP.NET Core Razor Pages
- .NET 8 LTS
- Entity Framework Core 8
- SQL Server / Azure SQL provider
- Bootstrap
- Built-in ASP.NET Core logging

## Important note about .NET version

The original target requested ASP.NET Core 9, but this machine only has the .NET 8 SDK installed. To ensure the app builds successfully today, the project is implemented on `net8.0` with the same architecture and Azure deployment model. Moving to .NET 9 later is straightforward once the .NET 9 SDK is installed.

## Folder structure

```text
APP/
├── .config/
│   └── dotnet-tools.json
├── FitAppReservations.sln
├── README.md
├── .gitignore
└── FitAppReservations/
    ├── Data/
    ├── Migrations/
    ├── Models/
    ├── Pages/
    │   ├── Clients/
    │   ├── Reservations/
    │   ├── Shared/
    │   └── Trainers/
    ├── Properties/
    ├── Services/
    ├── wwwroot/
    ├── appsettings.json
    ├── appsettings.Development.json
    ├── FitAppReservations.csproj
    └── Program.cs
```

## Local prerequisites

- .NET 8 SDK
- SQL Server LocalDB, SQL Server Express, or a reachable SQL Server / Azure SQL instance

## Local run instructions

1. Restore tools and packages:

   ```powershell
   dotnet tool restore
   dotnet restore
   ```

2. Review the development connection string in `FitAppReservations/appsettings.Development.json`.

3. If you have SQL Server LocalDB installed, run:

   ```powershell
   dotnet dotnet-ef database update --project .\FitAppReservations\FitAppReservations.csproj --startup-project .\FitAppReservations\FitAppReservations.csproj
   ```

4. Start the app:

   ```powershell
   dotnet run --project .\FitAppReservations\FitAppReservations.csproj
   ```

5. Open the local URL shown in the terminal.

## Database setup

The app uses EF Core Code First with SQL Server.

- Initial migration is included under `FitAppReservations/Migrations`
- Demo data is seeded only when the database is empty
- Startup migration behavior is controlled by:

```text
Database__ApplyMigrationsOnStartup
```

Recommended behavior:

- Local development: `true`
- Azure production: `false` by default, then enable temporarily for first deployment or run migrations manually

## Environment variables

The application reads configuration normally from ASP.NET Core configuration sources, including environment variables.

Required in Azure:

```text
ConnectionStrings__DefaultConnection
```

Optional:

```text
Database__ApplyMigrationsOnStartup
ASPNETCORE_ENVIRONMENT
```

## Azure App Service deployment notes

1. Create an Azure SQL Database and allow access from your App Service.
2. Create an Azure App Service for .NET.
3. Set this App Service application setting:

   ```text
   ConnectionStrings__DefaultConnection
   ```

4. Optionally set:

   ```text
   Database__ApplyMigrationsOnStartup=true
   ```

   Use it for the first deployment if your app identity or SQL login has permission to apply schema updates.

5. Deploy using Visual Studio publish, GitHub Actions, or Zip Deploy.
6. After the schema exists, you can set `Database__ApplyMigrationsOnStartup=false`.
7. Confirm the site and `/health` endpoint respond successfully.

## Azure SQL notes

- Use SQL authentication or a compatible secure connection strategy
- Keep `Encrypt=True` for Azure SQL
- Do not hardcode secrets in source control
- If you rotate credentials, update App Service settings instead of code

## Logging and observability

- ASP.NET Core logging is enabled through configuration
- Reservation validation failures are logged with structured properties
- Reservation create, update, and delete operations are logged
- Health checks are exposed at `/health`

## Validation summary

- Trainer hourly rate must be non-negative
- Client email is optional but validated when present
- Reservation end time must be later than start time
- Overlapping reservations for the same trainer are blocked
- Inactive trainers cannot receive non-cancelled reservations

## Commands reference

Restore tools:

```powershell
dotnet tool restore
```

Create a new migration:

```powershell
dotnet dotnet-ef migrations add YourMigrationName --project .\FitAppReservations\FitAppReservations.csproj --startup-project .\FitAppReservations\FitAppReservations.csproj --output-dir Migrations
```

Apply migrations:

```powershell
dotnet dotnet-ef database update --project .\FitAppReservations\FitAppReservations.csproj --startup-project .\FitAppReservations\FitAppReservations.csproj
```

Build:

```powershell
dotnet build .\FitAppReservations.sln
```

Run:

```powershell
dotnet run --project .\FitAppReservations\FitAppReservations.csproj
```

## Current verification

- `dotnet build .\FitAppReservations.sln` succeeded
- EF Core initial migration was generated successfully
- Local database update could not be executed in this environment because SQL Server LocalDB is not installed
