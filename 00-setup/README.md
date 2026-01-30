# 00 - Setup: Cost Control Baseline

## Goal
Prevent unexpected Azure charges while building an AZ-104 portfolio.

## What I configured
- Monthly budget: 15 US$
- Alerts at 50%, 80% and 100%
- Action group for cost notifications
- Tagging rules for all resources

## Tagging standard
- project=az104
- owner=bayron
- ttl=7d

## Cleanup rule
One Resource Group per lab. When finished, delete the Resource Group.

## Screenshots
- Budget created
- Budget alerts
- Action group
