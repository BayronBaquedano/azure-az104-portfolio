# 01 - Identity & RBAC

## Goal
Implement role-based access control using Azure Entra ID groups and scoped role assignments.

## Setup
- Created dedicated lab users
- Group-based RBAC model
- Resource Group scoped permissions

## Roles
- rg-readers: Reader role at Resource Group level
- rg-contributors: Contributor role at Resource Group level

## Validation
- Reader can view resources but cannot create them
- Contributor can create resources within the assigned scope

## Key takeaways
- RBAC should be assigned to groups, not users
- Scope matters: least privilege at Resource Group level
