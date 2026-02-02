# Notes – Project 02 Networking

## Key Learnings

### Network Segmentation
Separating application, database, and management traffic into different subnets reduces the attack surface and limits lateral movement in case of compromise.

### NSG Design
Each subnet has its own NSG to enforce security boundaries:
- Application subnet only exposes required public services (HTTP/HTTPS).
- Database subnet is completely isolated from Internet access.
- Management subnet is restricted to a specific admin IP.

### Protocol and Port Selection
Database communication uses TCP because it requires reliable, stateful connections. Only the required port (1433) is allowed, following least privilege principles.

### Default NSG Rules
Default NSG rules cannot be deleted and should not be removed. Custom rules with higher priority are used to override default behavior when needed.

### Real-World Relevance
This design reflects a common production setup in cloud environments and demonstrates practical security decision-making rather than lab-only configurations.
