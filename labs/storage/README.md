# Azure Storage Accounts

Storage Accounts are a managed storage service for data which you can make publicly accessible, or restricted to users or other Azure services. Data is replicated in multiple locations for high availability, and you can choose different levels of performance.

In this lab we'll expolore the basics of Storage Accounts, uploading small and large files.

## Reference

- [Storage Account overview](https://docs.microsoft.com/en-gb/azure/storage/common/storage-account-overview)

- [Data redundancy in Azure](https://docs.microsoft.com/en-gb/azure/storage/common/storage-redundancy?toc=%2Fazure%2Fstorage%2Fblobs%2Ftoc.json)

- [`az storage` commands](https://docs.microsoft.com/en-us/cli/azure/storage?view=azure-cli-latest)

- [`az storage account` commands](https://docs.microsoft.com/en-us/cli/azure/storage/account?view=azure-cli-latest)


## Explore Storage Account options

Create a new resource in the Portal, search for Storage Account. Look at the options you have:

- the name needs to be globally unique - and the [naming rules](https://docs.microsoft.com/en-us/azure/azure-resource-manager/management/resource-name-rules#microsoftstorage) are strict
- region is important, it's where the data is physically stored
- choose performance and redundancy levels

The redundancy level sets how the data is replicated:

- _Locally Redundant Storage_ (LRS) means the data is replicated in a single datacentre
- _Zone Redundant Storage_ (ZRS) replicates across datacentres in a single region
- _Geo-Redundant Storage_ (GRS) replicates across regions

Your data is more secure with wider replication, but that comes at higher cost.

## Create a storage account

We'll use the CLI to create a new Storage Account. Start with a Resource Group and then check the hel text for new accounts:

```
az group create -n labs-<your-name>-storage-RG  -l westeurope --tags courselabs=azure

az storage account create --help
```

📋 Create a zone-redundant storage account with standard performance.


The SKU parameter includes performance and redundancy settings, e.g:

- `Premium_LRS` is premium performance (SSD-backed storage) with local redundancy

- `Standard_GRS` is standard performance (spinning HDDs) with geo redundancy

```
az storage account create -g labs-<your-name>-storage-RG  -l westeurope --sku Standard_ZRS -n <your-name>storage
```

Open the new resource in the Portal - one storage account can support multiple types of storage. Blob storage (Binary Large OBjects) is a simple file storage option, where you can store files in _containers_, which are like folders.

📋 Upload the file [document.txt](/labs/storage/document.txt) in this folder as a blob in a container called _drops_.

The Storage Account blade has an _Upload_ option in the main menu. Select that and you can browse to your local file and upload it.

You can create a new container from that menu, and supply a container name.


> Blob storage is not hierarchical - you can't have containers in other containers - but blob names can include forward slashes e.g. `my/blob/file.txt` which lets you approximate nested storage

## Upload and download blobs

You can manage storage with a nice UI from within the portal. Click _Storage browser_ from the left nav and open _Blob containers_.

Open the `drops` and you'll see `document.txt`. Click and you'll get an overview which includes the URL. What is the URL of the file? Is is publicly accessible?

Use curl to download it:

```
# you won't get any errors here:
curl -o download2.txt https://<your-name>storage.blob.core.windows.net/drops/document.txt
```

It looks like the file has been downloaded. But check the contents:

```
cat download2.txt
```

> It's an XML error message... New blob containers default to private access. 

📋 Change the access level of the container so you can download the blob.

Browse to the _drops_ container in the Portal and select _Change access level_:

- blob access means anyone with the URL can download the file
- container access means anyone can list the container contents and download all blobs

Once you've set a public access level, you can download the file:

```
curl -o download3.txt https://<your-name>storage.blob.core.windows.net/drops/document.txt

cat download3.txt
```

Now the correct contents are there.


## Lab

Storage Accounts have a firewall option, similar to SQL Server in Azure. Use it to secure your original SA so it can only  be accessed from your own IP address. Confirm you can download the document.txt file; then login to your VM and confirm that it can't download the file.

> Stuck? Try [hints](hints.md) or check the [solution](solution.md).

___

## Cleanup

Delete the lab RG:

```
az group delete -y -n labs-<your-name>-storage-RG --no-wait
```
