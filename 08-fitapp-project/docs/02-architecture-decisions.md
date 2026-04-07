# Architecture Decisions

## Chosen services
- Azure App Service for web hosting
- Azure App Service Plan for compute
- Azure SQL Database for relational data
- Virtual Network for network segmentation
- One subnet for App Service VNet Integration
- One subnet for Private Endpoints
- Network Security Group for subnet-level traffic control
- Private Endpoint for Azure SQL access

## Why this design
This is a first real Azure project, so the design must be simple enough to build and explain, but realistic enough to show cloud engineering fundamentals:
- Infrastructure as Code
- PaaS hosting
- managed database
- private connectivity
- basic network security
- reproducible deployments

## Trade-offs
- No Azure Firewall in v1 to avoid unnecessary cost and complexity
- No Key Vault in v1 to keep the first deployment manageable
- No CI/CD yet; deployment will be manual first