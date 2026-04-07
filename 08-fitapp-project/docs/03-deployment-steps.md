# 03 - Deployment Steps

## Block 2 - Network and NSG

In this block, the following resources were defined and deployed with Bicep:
- Virtual Network
- App Service integration subnet
- Private Endpoints subnet
- Network Security Group

### Purpose
Create the base network structure for the project before deploying Azure SQL and App Service.

### Files created
- bicep/main.bicep
- bicep/main.parameters.json
- bicep/modules/network.bicep
- bicep/modules/nsg.bicep


## Block 3 - Azure SQL and Private Endpoint

In this block, the following resources were defined and deployed with Bicep:
- Azure SQL logical server
- Azure SQL database
- Private Endpoint for Azure SQL
- Private DNS zone for SQL private name resolution
- Virtual network link to the Private DNS zone

### Purpose
Deploy a managed relational database and expose it privately inside the virtual network.

### Files created or updated
- bicep/modules/sql.bicep
- bicep/main.bicep
- bicep/main.parameters.json


Azure App Service
   |
   |-- VNet Integration --> snet-appsvc-dev-we-01
                             |
                             v
                        vnet-fitapp-dev-we-01
                             |
                             v
                     snet-pe-dev-we-01
                             |
                             +--> Private Endpoint for Azure SQL
                             |
                             +--> NSG
                             
Azure SQL Server
   |
   +--> Azure SQL Database

Private DNS Zone
   |
   +--> linked to VNet
   |
   +--> zone group attached to Private Endpoint


## Block 4 - App Service

In this block, the following resources were deployed:
- App Service Plan
- Web App
- VNet integration with the application subnet

### Purpose
Provide a compute environment to host the web application and enable private connectivity to backend services.

### Validation evidence
Screenshots were captured for the App Service Plan, Web App overview, browser access, VNet integration, and final resource group state.

## Block 5 - Application Deployment and Configuration

In this block, the web application was published locally and deployed to Azure App Service.

### Tasks performed
- Published the ASP.NET application
- Created a ZIP package
- Deployed the package to Azure App Service
- Added the database connection string in App Service
- Tested the application endpoint and /health endpoint

### Purpose
Validate that the application can run in App Service and start using cloud configuration.

### Issue 1 - SQL login failure
- The app failed to start due to incorrect SQL credentials.
- Fix: reset SQL admin password and updated App Service connection string.

### Issue 2 - Missing database schema
- The app connected successfully but failed due to missing tables.
- Logs showed pending EF Core migrations.
- Fix: enabled `Database__ApplyMigrationsOnStartup=true`.


## Block 6 - Monitoring and observability

In this block, Application Insights was added to the Azure environment to provide monitoring and observability for the deployed web application.

### Resources and configuration added
- Application Insights resource
- App Service application setting:
  - `APPLICATIONINSIGHTS_CONNECTION_STRING`
- ASP.NET Core Application Insights integration in `Program.cs`

### Purpose
Enable real-time monitoring, request tracking, traces, and failure visibility directly from Azure.

### Deployment steps
1. Added a new Bicep module for Application Insights.
2. Updated `main.bicep` to deploy the monitoring resource.
3. Passed the Application Insights connection string into App Service through application settings.
4. Verified that `APPLICATIONINSIGHTS_CONNECTION_STRING` appeared in App Service environment variables.
5. Installed the `Microsoft.ApplicationInsights.AspNetCore` package in the application project.
6. Updated `Program.cs` to include:
   - `builder.Services.AddApplicationInsightsTelemetry();`
   - `builder.Logging.AddConsole();`
7. Published the application again.
8. Recreated the deployment ZIP package.
9. Redeployed the application to Azure App Service.
10. Validated telemetry in Application Insights using:
    - Live Metrics
    - Logs (`requests` and `traces`)
    - Failures

### Issues found during deployment

#### 1. Resource provider not registered

- Symptom:
  Deployment of Application Insights failed.

- Root cause:
  The subscription did not have the required resource provider registered for monitoring resources.

- Fix applied:
  Registered the following providers with Azure CLI:
  - `Microsoft.Insights`
  - `Microsoft.OperationalInsights`

- Result:
  The monitoring deployment could proceed correctly after provider registration.

---

#### 2. Deployment command executed from wrong directory

- Symptom:
  Azure CLI could not find `bicep/main.bicep`.

- Root cause:
  The deployment command was executed from the wrong directory instead of the project root.

- Fix applied:
  Changed to the project root directory before running the deployment command again.

- Result:
  Azure CLI was able to resolve the Bicep template path correctly.

---

#### 3. Shell syntax mismatch during deployment

- Symptom:
  CLI deployment commands failed with syntax errors.

- Root cause:
  Bash-style multiline syntax (`\`) was used inside PowerShell.

- Fix applied:
  Re-ran the command either:
  - in a single line, or
  - using PowerShell-compatible multiline syntax

- Result:
  The deployment command executed correctly.

---

#### 4. Application startup failed after monitoring changes

- Symptom:
  The application returned HTTP 500.30 after redeployment.

- Root cause:
  The `appsettings.json` file contained a duplicate configuration key:
  `Logging:LogLevel:Default`.

- Fix applied:
  Removed the duplicate key and kept a single valid `Logging` section.

- Result:
  The application configuration loaded correctly again.

---

#### 5. App Service logs were not generated initially

- Symptom:
  No useful stdout logs were being generated in Kudu.

- Root cause:
  `web.config` stdout logging configuration was not correctly set for App Service diagnostics.

- Fix applied:
  Updated the `aspNetCore` element in `web.config` to enable stdout logging and point it to the App Service log folder.

- Result:
  Application startup logs could be generated and inspected when needed.

---

#### 6. Application Insights resource existed, but the app was not sending telemetry

- Symptom:
  Live Metrics showed:
  `Not available: couldn't connect to your application`

- Root cause:
  The Application Insights resource and App Service setting existed, but the application code did not yet include:
  `builder.Services.AddApplicationInsightsTelemetry();`

- Fix applied:
  Added Application Insights telemetry registration to `Program.cs`, republished the app, recreated the ZIP package, and redeployed.

- Result:
  The application started sending telemetry to Application Insights.