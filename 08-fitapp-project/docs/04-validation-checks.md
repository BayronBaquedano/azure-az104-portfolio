# 04 - Validation Checks

## Block 2 - Network and NSG
- [x] Resource group exists
- [x] Virtual network deployed
- [x] App Service subnet deployed
- [x] Private Endpoints subnet deployed
- [x] NSG deployed
- [x] NSG associated with Private Endpoints subnet

## Block 3 - Azure SQL and Private Endpoint
- [x] Azure SQL logical server deployed
- [x] Azure SQL database deployed
- [x] Public network access disabled or restricted according to design
- [x] Private Endpoint deployed
- [x] Private DNS zone deployed
- [x] DNS zone linked to VNet
- [x] Private Endpoint connected successfully

## Block 4 - App Service
- [x] App Service Plan deployed
- [x] Web App deployed
- [x] Web App accessible from browser
- [x] VNet integration configured

## Block 5 - Application Deployment and Configuration
- [x] Application published locally
- [x] ZIP package deployed to App Service
- [x] Web application starts successfully
- [x] /health endpoint responds
- [x] Database connection string configured in App Service

## Block 6 - Monitoring and observability

### Resource validation
- [x] Application Insights resource deployed
- [x] Application Insights visible in the resource group
- [x] App Service contains `APPLICATIONINSIGHTS_CONNECTION_STRING` in application settings

### Application validation
- [x] Application starts successfully after telemetry integration
- [x] Main application pages remain accessible after monitoring changes
- [x] No critical startup issues remain after fixing configuration errors

### Monitoring validation
- [x] Application Insights Live Metrics accessible
- [x] Requests visible in Application Insights Logs
- [x] Traces visible in Application Insights Logs
- [x] Failures section accessible for diagnostics

### Operational validation
- [x] Subscription providers required for monitoring were registered successfully
- [x] Monitoring deployment completed successfully after fixing provider registration issue