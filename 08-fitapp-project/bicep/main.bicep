targetScope = 'resourceGroup'

@description('Azure region for all resources')
param location string = resourceGroup().location

@description('Project short name')
param projectName string

@description('Environment name')
param environmentName string

@description('Resource tags')
param tags object

@description('Virtual network name')
param vnetName string

@description('VNet address space')
param vnetAddressSpace string

@description('App Service integration subnet name')
param appSvcSubnetName string

@description('App Service integration subnet prefix')
param appSvcSubnetPrefix string

@description('Private Endpoints subnet name')
param privateEndpointsSubnetName string

@description('Private Endpoints subnet prefix')
param privateEndpointsSubnetPrefix string

@description('NSG name for private endpoints subnet')
param nsgName string

@description('Azure SQL logical server name')
param sqlServerName string

@description('Azure SQL database name')
param sqlDatabaseName string

@description('Azure SQL admin login')
@secure()
param sqlAdministratorLogin string

@description('Azure SQL admin password')
@secure()
param sqlAdministratorLoginPassword string

@description('Private endpoint name for Azure SQL')
param sqlPrivateEndpointName string

@description('App Service Plan name')
param appServicePlanName string

@description('Web App name')
param webAppName string

@description('Application Insights name')
param appInsightsName string

module nsg './modules/nsg.bicep' = {
  name: 'deploy-nsg'
  params: {
    location: location
    nsgName: nsgName
    tags: tags
  }
}

module network './modules/network.bicep' = {
  name: 'deploy-network'
  params: {
    location: location
    vnetName: vnetName
    vnetAddressSpace: vnetAddressSpace
    appSvcSubnetName: appSvcSubnetName
    appSvcSubnetPrefix: appSvcSubnetPrefix
    privateEndpointsSubnetName: privateEndpointsSubnetName
    privateEndpointsSubnetPrefix: privateEndpointsSubnetPrefix
    privateEndpointsNsgId: nsg.outputs.nsgId
    tags: tags
  }
}

module sql './modules/sql.bicep' = {
  name: 'deploy-sql'
  params: {
    location: location
    sqlServerName: sqlServerName
    sqlDatabaseName: sqlDatabaseName
    administratorLogin: sqlAdministratorLogin
    administratorLoginPassword: sqlAdministratorLoginPassword
    privateEndpointName: sqlPrivateEndpointName
    vnetId: network.outputs.vnetId
    privateEndpointsSubnetResourceId: network.outputs.privateEndpointsSubnetResourceId
    tags: tags
  }
}

module appInsightsModule './modules/appinsights.bicep' = {
  name: 'deploy-appinsights'
  params: {
    location: location
    appInsightsName: appInsightsName
    tags: tags
  }
}

module appservice './modules/appservice.bicep' = {
  name: 'deploy-appservice'
  params: {
    location: location
    appServicePlanName: appServicePlanName
    webAppName: webAppName
    appServiceSubnetResourceId: network.outputs.appSvcSubnetResourceId
    appInsightsConnectionString: appInsightsModule.outputs.connectionString
    tags: tags
  }
}

output vnetName string = network.outputs.vnetName
output vnetId string = network.outputs.vnetId
output appSvcSubnetResourceId string = network.outputs.appSvcSubnetResourceId
output privateEndpointsSubnetResourceId string = network.outputs.privateEndpointsSubnetResourceId
output nsgName string = nsg.outputs.nsgName
output sqlServerName string = sql.outputs.sqlServerName
output sqlDatabaseName string = sql.outputs.sqlDatabaseName
output sqlFqdn string = sql.outputs.sqlFqdn
output webAppName string = appservice.outputs.webAppName
output webAppUrl string = appservice.outputs.webAppUrl
output appInsightsName string = appInsightsModule.outputs.appInsightsName
output appInsightsConnectionString string = appInsightsModule.outputs.connectionString
