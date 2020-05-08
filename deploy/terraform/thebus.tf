#
# TheBus setup a single topic with two subscriptions
#

provider "azurerm" {
  # The "feature" block is required for AzureRM provider 2.x. 
  # If you are using version 1.x, the "features" block is not allowed.
  version = "~>2.0"
  features {}
}

# NOTE: In order to change attributes of a resource group, usually means you have to destroy it and then build out everything in it.

resource "azurerm_resource_group" "rg" {
  name     = var.resourceGroupName
  location = var.resourceLocationName

  tags = {
    source = "Terraform"
  }
}

# REF: https://www.terraform.io/docs/providers/azurerm/r/servicebus_namespace.html

resource "azurerm_servicebus_namespace" "bus" {
  name                = var.namespaceName
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "Standard"

  tags = {
    source = "Terraform"
  }
}
