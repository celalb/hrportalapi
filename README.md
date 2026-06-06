# HR Portal API

ASP.NET Core Web API backend skeleton for HR requests with configuration-driven SQL Server / SAP HANA data provider support.

## Features

- Layered structure (Controllers, DTOs, Models, Services, Repositories, Infrastructure, Configuration)
- Configurable database provider (`SqlServer` or `Hana`) from `appsettings.json`
- Endpoints:
  - `POST /api/leave-requests`
  - `POST /api/worklogs`
  - `POST /api/advance-requests`
  - `GET /api/requests/my`
  - `GET /api/admin/requests`
  - `PUT /api/admin/requests/{id}/status`
- Pending / Approved / Rejected status tracking
- CORS from configuration
- Swagger/OpenAPI enabled
- Exception handling scaffolding via middleware
- Current employee abstraction (`ICurrentEmployeeContext`) ready for future token/claim integration

## Configuration

Set `DatabaseSettings.Provider` in `appsettings.json`:

- `SqlServer` uses `DatabaseSettings.SqlServerConnectionString`
- `Hana` uses `DatabaseSettings.HanaConnectionString` and registered provider invariant name (`Sap.Data.Hana` by default)

## Database DDL

- SQL Server: `DatabaseScripts/sqlserver-requests.sql`
- SAP HANA: `DatabaseScripts/hana-requests.sql`

## Run

```bash
dotnet restore
dotnet run
```

Swagger UI will be available at `/swagger`.
