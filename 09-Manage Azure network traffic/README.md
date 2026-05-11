# AZ-104 Lab - Hub-Spoke Network, UDR, Azure Load Balancer and Application Gateway

## Overview

This project documents an Azure networking lab focused on traffic management across virtual networks and virtual machines.

The lab is based on a **Hub-Spoke network topology** and includes user-defined routing, network connectivity testing, Layer 4 load balancing with Azure Load Balancer, and Layer 7 traffic distribution with Azure Application Gateway.

The main goal was to understand how Azure routes traffic between virtual networks and how different load balancing services behave in real cloud scenarios.

---

## Objectives

The objectives of this lab were to:

- Deploy the lab environment using ARM templates.
- Create a Hub-Spoke virtual network topology.
- Configure virtual network peering.
- Test VNet peering transitivity behavior.
- Configure User Defined Routes, UDRs.
- Use a virtual machine as a Network Virtual Appliance, NVA.
- Deploy Azure Load Balancer for Layer 4 traffic distribution.
- Deploy Azure Application Gateway for Layer 7 application traffic.
- Validate network connectivity using Azure Network Watcher.

---

## Azure Services Used

- Azure Virtual Network
- Azure Virtual Machines
- Azure Network Interface
- Azure Network Watcher
- Azure Route Table
- User Defined Routes, UDR
- IP Forwarding
- Azure Load Balancer
- Azure Application Gateway
- Public IP Address
- Network Security Groups
- ARM Templates
- Azure Cloud Shell with PowerShell

---

## Architecture

The lab environment contains a Hub virtual network and two Spoke virtual networks.

```text
Hub VNet
├── VM0
├── VM1

Spoke VNet 2
└── VM2

Spoke VNet 3
└── VM3
```

The Hub VNet is connected to each Spoke VNet using VNet peering.

By default, VNet peering is not transitive. This means that even if Spoke VNet 2 and Spoke VNet 3 are both connected to the Hub, they cannot automatically communicate with each other.

To solve this, User Defined Routes were configured to force traffic between the Spokes through VM0, which acts as a Network Virtual Appliance.

---

## Task 1 - Environment Provisioning

The environment was provisioned using an ARM template and a parameters file.

A resource group was created first:

```powershell
New-AzResourceGroup -Name $rgName -Location $location1
```

Then the ARM template was deployed:

```powershell
New-AzResourceGroupDeployment `
  -ResourceGroupName $rgName `
  -TemplateFile az104-06-template.json `
  -TemplateParameterFile az104-06-parameters.json
```

The deployment created the virtual networks, virtual machines, network interfaces and supporting resources required for the lab.

---

## Task 2 - Network Watcher Extension

After provisioning the environment, the Network Watcher extension was installed on the virtual machines.

This allowed connectivity tests to be performed from the Azure portal using **Connection Troubleshoot**.

This was useful to verify whether one VM could reach another VM over a specific protocol and port, such as TCP 3389 for RDP.

---

## Task 3 - Hub-Spoke VNet Peering

VNet peering was configured between the Hub VNet and each Spoke VNet.

The main peerings were:

```text
VNet1 <-> VNet2
VNet1 <-> VNet3
```

Forwarded traffic was allowed in the peering configuration.

No gateway transit was configured in this lab.

Key concept:

```text
VNet peering is not transitive by default.
```

This means that VNet2 cannot automatically communicate with VNet3 just because both are peered with VNet1.

---

## Task 4 - Connectivity Testing

Connectivity was tested using Azure Network Watcher.

The expected results were:

```text
VM0 -> VM1 : Reachable
VM0 -> VM2 : Reachable
VM0 -> VM3 : Reachable
VM2 -> VM3 : Not reachable
```

The failed connection between VM2 and VM3 was expected because there was no direct peering between the two Spoke VNets and VNet peering does not provide automatic transitive routing.

---

## Task 5 - User Defined Routes and IP Forwarding

To enable traffic between the Spoke VNets, User Defined Routes were configured.

VM0 was used as a Network Virtual Appliance.

IP Forwarding was enabled on the VM0 network interface.

Windows routing features were also enabled inside the VM operating system.

### Route from VNet2 to VNet3

A route table was created with a route similar to:

```text
Destination: 10.63.0.0/20
Next hop type: Virtual appliance
Next hop IP: 10.60.0.4
```

This route table was associated with the relevant subnet in VNet2.

### Route from VNet3 to VNet2

A second route table was created with a route similar to:

```text
Destination: 10.62.0.0/20
Next hop type: Virtual appliance
Next hop IP: 10.60.0.4
```

This route table was associated with the relevant subnet in VNet3.

After this configuration, traffic between the Spoke VNets was routed through VM0.

Logical flow:

```text
VM2
↓
Route Table
↓
VM0 as NVA
↓
VM3
```

---

## Task 6 - Azure Load Balancer

A public Azure Load Balancer was deployed in front of two virtual machines.

Azure Load Balancer operates at Layer 4 and distributes traffic based on protocol and port.

Main configuration:

- Public Load Balancer
- Standard SKU
- Public frontend IP
- Backend pool with VM0 and VM1
- TCP health probe on port 80
- Load balancing rule for TCP port 80

Traffic flow:

```text
Internet
↓
Load Balancer Public IP
↓
Frontend IP configuration
↓
Load balancing rule
↓
Backend pool
↓
VM0 / VM1
```

The deployment was validated by accessing the public IP address of the Load Balancer from a browser.

---

## Task 7 - Azure Application Gateway

Azure Application Gateway was deployed in front of backend virtual machines.

Application Gateway operates at Layer 7 and is designed for HTTP and HTTPS application traffic.

Before creating the Application Gateway, a dedicated subnet was added to the virtual network.

Important requirement:

```text
Application Gateway requires a dedicated subnet.
```

Main configuration:

- Standard_v2 SKU
- Public frontend IP
- Dedicated Application Gateway subnet
- Backend pool using private IP addresses
- HTTP listener on port 80
- HTTP backend settings on port 80
- Routing rule with priority

Traffic flow:

```text
Internet
↓
Application Gateway Public IP
↓
HTTP Listener
↓
Routing Rule
↓
Backend Pool
↓
Backend VMs
```

The deployment was validated by accessing the public IP address of the Application Gateway from a browser.

---

## Azure Load Balancer vs Application Gateway

| Service | OSI Layer | Main Use Case |
|---|---:|---|
| Azure Load Balancer | Layer 4 | TCP/UDP traffic distribution |
| Azure Application Gateway | Layer 7 | HTTP/HTTPS application traffic routing |

Azure Load Balancer is suitable when the requirement is to distribute traffic based on IP, protocol and port.

Azure Application Gateway is suitable when application-level routing is required, such as HTTP listeners, path-based routing, host-based routing, SSL termination or Web Application Firewall integration.

---

## Key Learnings

- Hub-Spoke is a common enterprise network topology in Azure.
- VNet peering is not transitive by default.
- User Defined Routes can be used to control traffic flow explicitly.
- Route tables are associated with subnets, not directly with virtual machines.
- IP Forwarding must be enabled when a VM is used as a router or NVA.
- The VM operating system must also be configured to forward traffic.
- Azure Network Watcher is useful for validating and troubleshooting connectivity.
- Azure Load Balancer works at Layer 4.
- Azure Application Gateway works at Layer 7.
- Application Gateway requires a dedicated subnet.

---

## Conclusion

This lab helped reinforce core Azure networking concepts required for the AZ-104 certification and for real-world cloud engineering scenarios.

The most important concepts practiced were VNet peering, Hub-Spoke topology, non-transitive routing, User Defined Routes, traffic forwarding through an NVA, Azure Load Balancer and Azure Application Gateway.

This project demonstrates practical knowledge of Azure networking, routing, diagnostics and traffic distribution.
