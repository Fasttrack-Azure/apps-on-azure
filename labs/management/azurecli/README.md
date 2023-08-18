# Azure Management using PowerShell and Azure CLI
 
## Use the PowerShell CLI
Launch the cloud shell from the Azure Portal
<img width="1303" alt="image" src="https://user-images.githubusercontent.com/11691661/179504416-f2175247-d747-491c-88e2-2723ede65d35.png">

Half the screen will be in PowerShell command line interface (CLI) mode. If you’re familiar with PowerShell, you can manage your Azure environment using PowerShell commands.

<img width="1440" alt="image" src="https://user-images.githubusercontent.com/11691661/179504508-50d24e73-d824-48d6-957d-e740dfce3eb5.png">

```
**Tip**
You can tell you're in PowerShell mode by the PS before your directory on the command line.
```

Use the PowerShell Get-date command to get the current date and time.

```
Get-date
```

Most Azure specific commands will start with the letters az. The Get-date command you just ran is a PowerShell specific command. Let's try an Azure command to check what version of the CLI you're using right now.

```
az version
```

## Use the BASH CLI
If you’re more familiar with BASH, you can use BASH command instead by shifting to the BASH CLI.

Enter bash to switch to the BASH CLI.

```
bash
```

```
**Tip**
You can tell you're in BASH mode by the username displayed on the command line. It will be your username@azure.
```

Again, use the Get-date command to get the current date and time.

```
Get-date
```

You received an error because Get-date is a PowerShell specific command.

Screenshot of BASH error message get-date command not found.
<img width="1440" alt="image" src="https://user-images.githubusercontent.com/11691661/179505428-d8c0d1f1-f818-4db4-b25a-3fd3e4f9b0f8.png">Use the date command to get the current date and time.

```
date
```


## Finding commands

Azure CLI commands are organized as commands of groups. Each group represents an Azure service, and commands operate on that service.
To search for commands, use az find. For example, to search for command names containing secret, use the following command:

```
az find secret
```
 
Use the --help argument to get a complete list of commands and subgroups of a group. For example, to find the CLI commands for working with Network Security Groups (NSGs):

```
az network nsg --help
```

Interactive mode

The CLI offers an interactive mode that automatically displays help information and makes it easier to select subcommands. You enter interactive mode with the az interactive command.

```
az interactive
```
 
# Managing subscriptions using Azure CLI
 
## Change the active subscription
 ```
 # change the active subscription using the subscription name
az account set --subscription "Azure Scalability Class"

# change the active subscription using the subscription ID
az account set --subscription "79080195-8aac-4282-8f0d-5910cb4209c0"

 ```
 
 ## Create Management Groups
 You can create a management group for several of your subscriptions by using the az account management-group create command:

```
az account management-group create --name <mg-yourname>
 ```
 
To see all your management groups, use the az account management-group list command:

```
az account management-group list
 ```
 
## Add subscriptions to your new group by using the az account management-group subscription add command:

```
az account management-group subscription add --name <mg-yourname> --subscription "Azure Scalability Class"
 ```
 
## To remove a subscription, use the az account management-group subscription remove command:

```
az account management-group subscription remove --name <mg-yourname> --subscription "Azure Scalability Class"
 ```
 
## To remove a management group, run the az account management-group delete command:

```
az account management-group delete --name mg-yourname
 ```
 
Removing a subscription or deleting a management group doesn't delete or deactivate a subscription.
 
 
When you remove a resource group, you delete all the resources that belong to it. There's no option to undelete resources. If you try any of the commands in this article, deleting the resource groups you create cleans up your account.
 
# Common Azure CLI commands
 
This below list shows some common commands used in the CLI and links to their reference documentation.


Resource group	
 ```
 az group
 ```
 
Virtual machines	
 ```
 az vm
 ```
Storage accounts	
 ```
 az storage account
 ```
 
Key Vault	
 ```
 az keyvault
 ```
 
Web applications	
 ```
 az webapp
 ```
 
SQL databases	
 ```
 az sql server
 ```
 
CosmosDB	
 ```
 az cosmosdb
 ```
