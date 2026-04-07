param location string
param appServicePlanName string
param webAppName string
param appServiceSubnetResourceId string
param appInsightsConnectionString string
param tags object = {}

resource appServicePlan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: appServicePlanName
  location: location
  tags: tags
  sku: {
    name: 'B1'
    tier: 'Basic'
    capacity: 1
  }
  properties: {
    reserved: false
  }
}

resource webApp 'Microsoft.Web/sites@2024-04-01' = {
  name: webAppName
  location: location
  tags: tags
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      alwaysOn: true
      appSettings: [
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsightsConnectionString
        }
      ]
    }
  }
}

resource vnetIntegration 'Microsoft.Web/sites/networkConfig@2024-04-01' = {
  parent: webApp
  name: 'virtualNetwork'
  properties: {
    subnetResourceId: appServiceSubnetResourceId
    swiftSupported: true
  }
}

output webAppName string = webApp.name
output webAppUrl string = 'https://${webApp.properties.defaultHostName}'
