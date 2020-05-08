variable "commonAuditSubscriptionName" {
  default = "audit"
  type    = string
}

variable "environmentName" {
  default = "dev"
  type    = string
}

variable "namespaceName" {
  default = "TheBusStop"
  type    = string
}

# NOTE: In order to change attributes of a resource group, usually means you have to destroy it and then build out everything in it.

variable "resourceGroupName" {
  default = "TheBusRG"
  type    = string
}

variable "resourceLocationName" {
  default = "South Central US"      ## NOTE: Terraform will convert this to "southcentralus", but keep these files as human readable as possible.
  type    = string
}