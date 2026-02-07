# Project 03 – Azure Compute (Virtual Machines)

## Overview
In this project I deployed and secured a Linux Virtual Machine in Microsoft Azure as part of my AZ-104 preparation.

The goal was to understand how compute resources integrate with networking and security components, and how to deploy a cost-efficient VM suitable for lab environments.

---

## Architecture
- Virtual Network (existing)
- Subnet: `snet-app`
- Linux VM (Ubuntu Server 24.04 LTS)
- Network Interface (NIC)
- Network Security Group (NSG) associated to the NIC

---

## Key Configuration
- **VM OS:** Ubuntu Server 24.04 LTS
- **VM Size:** B1s (1 vCPU, 1 GiB RAM – low cost)
- **Authentication:** SSH key-based authentication
- **Public IP:** Enabled (for lab access only)
- **NSG Rules:**
  - Allow SSH (22) only from my public IP
  - Deny all other inbound traffic by default

---

## Security Considerations
- Password authentication disabled
- SSH access restricted by source IP
- NSG applied at NIC level for granular control

---

## Validation
- Successful SSH connection from local machine
- Verified network configuration and hostname inside the VM
- Confirmed NSG rules behavior

---

## Cost Control
- Low-cost VM size selected
- Resources deleted after validation to avoid unnecessary charges

---

## Skills Demonstrated
- Azure Virtual Machines
- Linux VM deployment
- SSH authentication
- NSG configuration
- Cost-aware resource selection
- Basic troubleshooting

---

## Screenshots
See the `screenshots/` folder for deployment and validation evidence.
