## App startup and database connectivity
The application was initially created to run with a local SQL Server setup.
When moving to Azure, the startup process and connection strategy had to be validated against Azure SQL and App Service configuration.Compress-Archive -Path .\publish\* -DestinationPath .\fitapp.zip -Force

## Real-world debugging in Azure App Service

- Application startup failures require checking App Service logs, not only browser errors.
- A successful deployment does not guarantee a working system.
- Database connectivity and schema initialization are separate problems.
- EF Core migrations must be handled explicitly in cloud environments.

## Critical lesson

The biggest issue was not infrastructure, but application behavior during startup.
Understanding logs and tracing root causes was essential to fix the system.


### Issues found during deployment

#### 1. Application startup failure (HTTP 500 / 503)

- Symptom:
  The application returned HTTP 500.30 and later HTTP 503 errors after deployment.

- Root cause:
  The issue was initially suspected to be related to Azure SQL connection or App Service configuration.
  However, after reviewing logs, the real cause was a failure during application startup due to an EF Core query that could not be translated to SQL.

- Fix applied:
  The LINQ queries in `/Trainers` and `/Clients` were refactored:
  - Filtering is now applied before projection
  - Projection moved to the end of the query
  - Navigation property usage (`Reservations.Count`) replaced with explicit subqueries using `dbContext.Reservations`

- Result:
  The application started successfully and all endpoints became accessible.

---

#### 2. Deployment failure due to invalid ZIP path

- Symptom:
  Deployment failed with:
  "is not a valid local file path or you do not have permissions"

- Root cause:
  The ZIP file (`fitapp.zip`) did not exist in the expected directory or incorrect path syntax was used (`.\` vs `./`).

- Fix applied:
  - Recreated the ZIP file from the `/publish` folder
  - Used correct path syntax for Bash environment (`./fitapp.zip`)

- Result:
  Deployment completed successfully.

---

#### 3. Environment mismatch (PowerShell vs Bash)

- Symptom:
  Commands failed or behaved inconsistently.

- Root cause:
  Mixing Windows-style paths (`.\`) with Bash environment (MINGW64).

- Fix applied:
  Standardized command usage to Bash-compatible syntax (`./`).

- Result:
  CLI commands executed correctly.

  ### Observability is not optional

Without Application Insights, diagnosing issues required manual log inspection.

With monitoring enabled:
- Issues can be detected in real time
- Requests and failures can be analyzed without accessing the app
- Debugging becomes faster and more reliable

This highlights the importance of integrating observability early in any cloud-based application.

## Monitoring integration requires both infrastructure and application changes

Creating Application Insights in Azure is not enough by itself.

The monitoring resource must exist in Azure, the App Service must receive the `APPLICATIONINSIGHTS_CONNECTION_STRING`, and the ASP.NET Core application must explicitly enable telemetry with:

- `builder.Services.AddApplicationInsightsTelemetry();`

Without that application-side integration, Application Insights exists but receives no useful telemetry.

---

## Small configuration mistakes can break the whole application

A duplicate key in `appsettings.json` was enough to stop the application from starting.

Lesson:
Configuration changes must be treated with the same discipline as code changes.

---

## Cloud troubleshooting requires environment awareness

Several deployment issues were caused not by Azure services themselves, but by operational mistakes such as:
- wrong working directory
- wrong shell syntax
- missing provider registration

Lesson:
A large part of cloud engineering is not only architecture, but execution discipline.