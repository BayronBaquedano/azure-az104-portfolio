## Personal Notes – Project 03 (Compute)

### VM sizing
Initially, Azure suggested VM sizes that were too expensive for a lab.
I learned to manually filter VM sizes by:
- vCPUs
- RAM
- Monthly cost

For labs, B-series VMs (like B1s) are sufficient and cost-effective.

---

### SSH authentication
SSH failed at first because:
- The private key path was incorrect
- Permissions and key usage must match exactly

Key takeaway:
- Azure does NOT allow password login by default for Linux VMs (good security practice)
- SSH key management is critical

---

### NSG behavior
- Default NSG rules cannot be deleted
- Custom rules with higher priority override defaults
- Applying NSG at NIC level allows more precise control than subnet-level NSGs

---

### Security mindset
Public IPs should only be used for labs or controlled scenarios.
In production, access should be:
- Via Bastion
- Or via private connectivity (VPN / ExpressRoute)

---

### Cost awareness
Even simple compute resources can become expensive if left running.
Deleting resources after validation is mandatory in lab environments.

---

### Interview-ready explanation
"I deployed a Linux VM in Azure with SSH-only access, restricted by NSG rules, using a low-cost B-series VM to control costs. I validated connectivity, security rules, and removed all resources after testing."
