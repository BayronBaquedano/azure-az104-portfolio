Internet
   |
   v
[Azure App Service]
   |
   |-- Managed Identity --> [Azure Key Vault]
   |
   |-- Telemetry ---------> [Application Insights / Azure Monitor]
   |
   |-- VNet Integration --> [VNet: vnet-fitapp-dev-we-01]
                                |
                                |-- [Subnet: snet-appsvc-int-dev-we-01]
                                |      |
                                |      └─ usada por App Service VNet Integration
                                |
                                └-- [Subnet: snet-pe-dev-we-01]
                                       |
                                       ├─ [NSG: nsg-pe-dev-we-01]
                                       |
                                       └─ [Private Endpoint: Azure SQL]
                                              |
                                              v
                                       [Azure SQL Database]



```mermaid
flowchart TD
    U[Internet] --> A[Azure App Service]

    A --> VNI[VNet Integration]
    VNI --> VNET[VNet]

    VNET --> S1[Subnet - App Service Integration]
    VNET --> S2[Subnet - Private Endpoints]

    S2 --> NSG[NSG]
    NSG --> PE[Private Endpoint]
    PE --> SQL[Azure SQL Database]