param location string
param vnetName string
param vnetAddressSpace string
param appSvcSubnetName string
param appSvcSubnetPrefix string
param privateEndpointsSubnetName string
param privateEndpointsSubnetPrefix string
param privateEndpointsNsgId string
param tags object = {}

resource vnet 'Microsoft.Network/virtualNetworks@2025-01-01' = {
  name: vnetName
  location: location
  tags: tags
  properties: {
    addressSpace: {
      addressPrefixes: [
        vnetAddressSpace
      ]
    }
    subnets: [
      {
        name: appSvcSubnetName
        properties: {
          addressPrefix: appSvcSubnetPrefix
          delegations: [
            {
              name: 'webappDelegation'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'
              }
            }
          ]
        }
      }
      {
        name: privateEndpointsSubnetName
        properties: {
          addressPrefix: privateEndpointsSubnetPrefix
          privateEndpointNetworkPolicies: 'Disabled'
          networkSecurityGroup: {
            id: privateEndpointsNsgId
          }
        }
      }
    ]
  }
}

output vnetId string = vnet.id
output vnetName string = vnet.name
output appSvcSubnetResourceId string = '${vnet.id}/subnets/${appSvcSubnetName}'
output privateEndpointsSubnetResourceId string = '${vnet.id}/subnets/${privateEndpointsSubnetName}'
