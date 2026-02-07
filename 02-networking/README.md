# Project 02 – Azure Networking & NSG Design

## Overview
This project focuses on designing a secure and segmented virtual network in Azure using Virtual Networks, subnets, and Network Security Groups (NSGs).

The goal is to implement a layered network architecture following the principle of least privilege and preventing unnecessary lateral movement.

## Architecture
- Virtual Network with isolated subnets:
  - Application subnet (snet-app)
  - Database subnet (snet-db)
  - Management subnet (snet-mgmt)

- Dedicated NSG per subnet:
  - nsg-app: Allows HTTP/HTTPS traffic from Internet
  - nsg-db: Allows database access only from application subnet
  - nsg-mgmt: Allows management access only from admin IP

## Security Principles Applied
- Least privilege
- Network segmentation
- Controlled ingress and east-west traffic
- Explicit deny by default

## Technologies Used
- Azure Virtual Network
- Azure Network Security Groups
- Subnet-level NSG association

## Outcome
A secure and scalable network foundation suitable for real-world cloud environments.
