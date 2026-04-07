## Azure SQL - Cost and Security Notes

### Cost
The database uses a basic development-oriented SKU to keep costs low for a first project.

### Security
- Azure SQL is exposed through a Private Endpoint
- Private DNS is used for internal name resolution
- The design avoids relying on public access for application-to-database communication

### Monitoring and observability

Application Insights was integrated to provide real-time monitoring and logging.

Key capabilities:
- Request tracking
- Performance monitoring
- Failure detection
- Log querying using KQL

This allows debugging issues directly in Azure without relying on local reproduction.