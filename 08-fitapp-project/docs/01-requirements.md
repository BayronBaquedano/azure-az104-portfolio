# Project Requirements

## Scenario
A small booking web application for personal trainers will be deployed to Azure.

## Main goals
- Host a simple web application in Azure
- Use a managed relational database
- Keep the initial architecture simple but realistic
- Use Bicep as Infrastructure as Code
- Apply basic network segmentation
- Protect the database with a Private Endpoint
- Use an NSG in the network design
- Prepare the project for portfolio documentation

## Functional requirements
- The app must be publicly accessible
- The app must connect to a relational database
- The app must expose a `/health` endpoint
- The app must support trainer, client, and reservation data

## Non-functional requirements
- Low complexity for a first real Azure project
- Infrastructure must be repeatable using Bicep
- Naming convention must be consistent
- Project must be documented with screenshots and markdown