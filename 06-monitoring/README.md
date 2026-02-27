# Project 06 – Monitoring (Azure Monitor & Alerts)

## Overview

In this project, I implemented basic monitoring in Azure using Azure Monitor.  
The focus was on understanding how Azure collects data from a virtual machine and how to react to it using alerts.

---

## What I implemented

### Virtual Machine
- Created a Linux VM (`vm-az104-p06`) in Spain Central.
- Used it as the monitored resource.

### Azure Monitor (basic logs)
- Accessed logs through Azure Monitor.
- Verified connectivity using the `Heartbeat` table.
- Confirmed that the VM is reporting data to the monitoring system.

### Alert (CPU usage)
- Created an alert based on CPU percentage.
- Configured a threshold and evaluation period.
- Linked the alert to an Action Group (email notification).

### Testing
- Generated CPU load manually from the VM.
- Verified that the alert was triggered and notification was received.

---

## Validation

- `Heartbeat` query confirmed that the VM is connected and sending data.
- Alert successfully triggered when CPU threshold was exceeded.
- Email notification received through Action Group.

---

## Key points learned

- Azure Monitor provides basic data (like Heartbeat) without additional configuration.
- More advanced data (like performance counters) requires extra setup (DCR).
- Alerts are more useful than raw logs in real-world scenarios.
- Monitoring is not just about viewing data, but reacting to it.

---

## Technologies used

- Azure Monitor
- Log Analytics (basic usage)
- Azure Virtual Machines
- Alert Rules
- Action Groups

---

## Conclusion

This project helped me understand how monitoring works in Azure at a practical level:  
checking resource health, creating alerts, and validating real scenarios.