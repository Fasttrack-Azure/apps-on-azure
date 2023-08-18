# Working with Azure App Service

# App Service for Web Apps

IaaS options are great when you need access to the host machine to configure and deploy your app, but it leaves you with a lot of management overhead. Platform-as-a-Service (PaaS) takes care of that for you, simplifying deployment and updates, provided your application is supported in the PaaS environment. Azure has a few PaaS options - App Service is a very popular one.

In this lab you'll create an App Service deployment by pushing source code from your local machine. Azure will compile and configure the app for you.

## Reference

- [Azure App Service overview](https://docs.microsoft.com/en-us/azure/app-service/overview)

- [App Service Plan overview](https://docs.microsoft.com/en-us/azure/app-service/overview-hosting-plans)

- [`az appservice` commands](https://docs.microsoft.com/en-us/cli/azure/appservice?view=azure-cli-latest)

- [`az webapp` commands](https://docs.microsoft.com/en-us/cli/azure/webapp?view=azure-cli-latest)


## Create an ASP.NET Core app in a Docker container from Docker Hub using Azure CLI
### Launch Azure Cloud Shell Bash Terminal

The Azure Cloud Shell is a free interactive shell that you can use to run the steps in this article. It has common Azure tools preinstalled and configured to use with your account.
To open the Cloud Shell, just select Try it from the upper right corner of a code block. You can also launch Cloud Shell in a separate browser tab by going to https://shell.azure.com.

<img width="1440" alt="image" src="https://github.com/Fasttrack-Azure/Cloud-For-Partners/assets/11691661/d551459b-7d3f-43b7-9cfb-fc0d49538fc0">


### Sign in to Azure from CLI
```
$subscription="79080195-8aac-4282-8f0d-5910cb4209c0" # add subscription here

az account set -s $subscription # ...or use 'az login'
```
### To create the web app
```
# Create an ASP.NET Core app in a Docker container from Docker Hub
# set -e # exit if error
# Variable block
let "randomIdentifier=$RANDOM*$RANDOM"
location="East US"
resourceGroup="RG-<your-name>-$randomIdentifier"
tag="deploy-linux-docker-app-only.sh"
appServicePlan="sb-app-service-plan-$randomIdentifier"
webapp="sb-web-app-$randomIdentifier"
image="mcr.microsoft.com/dotnet/samples:aspnetapp"

# Create a resource group.
echo "Creating $resourceGroup in "$location"..."
az group create --name $resourceGroup --location "$location" --tag $tag

# Create an App Service plan in Free tier
echo "Creating $appServicePlan"
az appservice plan create --name $appServicePlan --resource-group $resourceGroup --sku Free --is-linux

# Create a web app. To see list of available runtimes, run 'az webapp list-runtimes --linux'
echo "Creating $webapp"
az webapp create --name $webapp --resource-group $resourceGroup --plan $appServicePlan  -i $image

# Copy the result of the following command into a browser to see the static HTML site.
site="http://$webapp.azurewebsites.net"
echo $site
curl "$site"
```

## Scale up your app service 
```
az appservice plan update --sku B1 --name $appServicePlan --resource-group $resourceGroup
```

## Scale out your app service 
```
az appservice plan update --number-of-workers 2 --name $appServicePlan --resource-group $resourceGroup
```


## Clean up resources
Use the following command to remove the resource group and all resources associated with it using the az group delete command - unless you have an ongoing need for these resources. Some of these resources may take a while to create, as well as to delete.
```
az group delete --name $resourceGroup
```
