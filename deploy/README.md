# :open_file_folder: deploy

## :file_folder: terraform

* :page_facing_up: thebus.tf
* :page_facing_up: thebust.tfvars

### Setup

* Download and install Terraform from [here](https://www.terraform.io/). It's just an executable file `terraform.exe` so make sure it's in the path.
* Download and install the Azure Command Line Interface (Az CLI) [here](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?view=azure-cli-latest)
* The Terraform VS Code extension is very handy and VS Code will prompt you when it sees that you're using `.tf` files.

### Azure CLI Reference

* `az login` - Prompts for your credentials using a browser.
* `az account list` - Information about your account(s).

### Terraform CLI Reference

* `terraform init` - This will download what Terraform needs based on the provider specified in the file.  These will be saved in a :file_folder: `.terraform` folder.
* `terraform plan` - Shows what **will** be created.
* `terraform apply` - Actually **creates** the resources.
* `terraform destroy` - **Destroys** what was created.

### Key components of a Terraform file

* Variables
* Resources
* Data Sources
* Outputs

### Deploying Infrastructure

* Repeatable
* Consistent

### State File

* Version
* Terraform Version
* Serial - The runninng count of changes
* Lineage
* Outputs
* Resources

## References

* [Quickstart: Install and configure Terraform to provision Azure resources](https://docs.microsoft.com/en-us/azure/developer/terraform/install-configure)
* https://www.terraform.io/docs/providers
