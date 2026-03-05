# Project 07 – Azure Backup (Recovery Services Vault)

## Overview

In this project I implemented backup and recovery for an Azure Virtual Machine using Azure Backup and a Recovery Services Vault.

The goal was to understand how Azure protects virtual machines and how recovery works if a resource fails.

---

## Infrastructure

Resources created:

- Resource Group: `rg-az104-p07-backup`
- Virtual Machine: `vm-az104-p07`
- Recovery Services Vault: `rsv-az104-p07`
- Backup Policy: daily backup with 7-day retention

Region used: **Spain Central**

---

## Backup Configuration

1. Created a Recovery Services Vault.
2. Configured Azure Backup for an Azure Virtual Machine.
3. Selected the VM to protect.
4. Created a backup policy (daily backup, 7 days retention).
5. Enabled backup for the VM.

After enabling the policy, a manual backup was executed to generate the first recovery point.

---

## Backup Validation

A manual backup was triggered using **Backup Now** and the job status was verified in **Backup Jobs**.

This confirmed that Azure successfully created a recovery point for the virtual machine.

---

## Restore Test

A restore operation was tested using **Restore Disks** from the recovery point.

This step validated that the VM data can be recovered from the Recovery Services Vault.

---

## Key Learnings

- Azure Backup stores recovery points inside a **Recovery Services Vault**.
- Backup policies define frequency and retention.
- A manual backup is useful to create the first recovery point immediately.
- Recovery can be performed even if the original VM is unavailable.

---

## Technologies Used

- Azure Virtual Machines
- Azure Backup
- Recovery Services Vault
- Backup Policies