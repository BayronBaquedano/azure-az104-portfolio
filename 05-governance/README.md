# Project 05 – Governance & Cost Control (Azure CLI)

## Overview

In this project, I focused on implementing basic governance and cost control mechanisms in Azure using Azure CLI. The goal was to simulate a more realistic cloud environment where resource usage, deployment rules, and protection need to be controlled.

---

## What I implemented

### Resource Group
Created a dedicated resource group to isolate all related components of the project and simplify cleanup.

### Action Group
Configured an Action Group to centralize alert notifications. This allows reuse across budgets and monitoring alerts instead of defining email notifications repeatedly.

### Budget (Subscription Level)
Set up a monthly budget with alert thresholds. This helps detect unusual spending early, although it does not prevent resource creation or usage.

### Policy – Allowed Locations
Assigned a policy to restrict resource deployment to a specific region (`centralspain`). This prevents accidental deployments in unintended or more expensive regions.

### Policy – Require Tag
Assigned a policy to require the presence of the `owner` tag on resources. This enforces basic organization and enables better cost tracking.

### Resource Lock
Applied a `CanNotDelete` lock to the resource group to prevent accidental deletion.

---

## Validation

- Tried to deploy a resource (Storage Account) in a non-allowed region → request was denied by policy.
- Verified that budget alerts are configured and linked to the Action Group.
- Confirmed that the resource lock prevents deletion of the resource group.

---

## Key points

- Policies apply differently depending on the resource type. For example, Resource Groups can still be created in other regions, but resource deployment inside them is restricted.
- Budgets do not stop spending; they only trigger alerts.
- Governance becomes more important as the number of resources grows.

---

## Technologies used

- Azure CLI (Bash)
- Azure Policy
- Azure Monitor
- Azure Cost Management

---

## Conclusion

This project helped me understand how to move from simply creating resources to actually controlling and governing them, which is closer to real-world cloud administration.