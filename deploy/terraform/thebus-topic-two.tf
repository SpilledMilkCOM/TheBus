
# REF: https://www.terraform.io/docs/providers/azurerm/r/servicebus_topic.html

resource "azurerm_servicebus_topic" "topic2" {
  name                = "topic-two"
  resource_group_name = azurerm_resource_group.rg.name
  namespace_name      = azurerm_servicebus_namespace.bus.name

  enable_partitioning = true
}

# REF: https://www.terraform.io/docs/providers/azurerm/r/servicebus_subscription.html

resource "azurerm_servicebus_subscription" "top2_sub1" {
  name                = "sub-one"
  resource_group_name = azurerm_resource_group.rg.name
  namespace_name      = azurerm_servicebus_namespace.bus.name
  topic_name          = azurerm_servicebus_topic.topic2.name
  max_delivery_count  = 1
}

resource "azurerm_servicebus_subscription" "top2_sub2" {
  name                = "sub-two"
  resource_group_name = azurerm_resource_group.rg.name
  namespace_name      = azurerm_servicebus_namespace.bus.name
  topic_name          = azurerm_servicebus_topic.topic2.name
  max_delivery_count  = 1
}

resource "azurerm_servicebus_subscription" "top2_audit" {
  name                = var.commonAuditSubscriptionName
  resource_group_name = azurerm_resource_group.rg.name
  namespace_name      = azurerm_servicebus_namespace.bus.name
  topic_name          = azurerm_servicebus_topic.topic2.name
  max_delivery_count  = 1
}