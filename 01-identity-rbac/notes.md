# Project 01 – Identity & RBAC (AZ-104)

## Objective
Implement and validate Azure role-based access control (RBAC) using Microsoft Entra ID users, groups, and scoped role assignments.  
The goal is to demonstrate least-privilege access, separation of duties, and real-world identity governance practices.

---

## Scope
- Subscription-level RBAC awareness
- Resource Group–level access control
- User vs Group role assignment comparison
- Access validation using non-admin identities

---

## Initial Setup
- Dedicated Resource Group created for the project:
  - Name: rg-az104-proj01
- RBAC configured at **Resource Group scope** to avoid over-permissioning.
- Administrative tasks performed using the main admin account.
- Validation performed using non-privileged users.

---

## Identity Configuration

### Users Created
- **rbac-admin**
  - Purpose: simulate delegated administrator access
- **rbac-reader**
  - Purpose: simulate read-only operational access

> These users were created to avoid using the global/admin account for day-to-day operations, following security best practices.

---

## Group Configuration

### Groups Created
- **grp-rg-admins**
  - Intended role: Contributor
- **grp-rg-readers**
  - Intended role: Reader

> RBAC assignments were performed at group level instead of user level to ensure scalability and easier access management.

---

## RBAC Assignments

### Role Assignments
| Scope | Principal | Role |
|------|----------|------|
| Resource Group | grp-rg-admins | Contributor |
| Resource Group | grp-rg-readers | Reader |

### Rationale
- **Contributor** allows full resource management without granting permission management.
- **Reader** ensures visibility without modification capabilities.
- No roles were assigned at subscription level to minimize blast radius.

---

## Access Validation

### Validation Steps
1. Signed in as `rbac-reader`
   - Verified read-only access
   - Confirmed inability to create, modify, or delete resources
2. Signed in as `rbac-admin`
   - Verified ability to create and manage resources
   - Confirmed inability to assign roles (no Owner rights)

### Result
RBAC behavior matched expected role definitions with no privilege escalation.

---

## Security Considerations
- Principle of Least Privilege applied
- Group-based RBAC preferred over direct user assignments
- Administrative account used only for configuration, not validation
- Clear separation between admin, contributor, and reader responsibilities

---

## Lessons Learned
- Group-based RBAC significantly simplifies access management
- Assigning roles at Resource Group scope provides strong isolation
- Testing access with real user accounts is essential to validate security posture
- RBAC misconfiguration is easy to detect when roles are properly scoped

---

## Clean-up Strategy
- All resources deployed under a dedicated Resource Group
- Resource Group deleted after project completion to avoid unnecessary costs
- Identities retained for future labs and reuse

---

## Project Status
✅ Completed  
This project fulfills the Identity and RBAC objectives of the AZ-104 certification and reflects real-world Azure administration practices.
