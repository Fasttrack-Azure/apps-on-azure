# This lab will help you create a .Net application using Azure PaaS services.

## Services used:

- [Azure App Services](https://azure.microsoft.com/en-us/products/app-service/)
- [Azure SQL](https://azure.microsoft.com/en-in/products/azure-sql/database/)
- [Azure Cache for Redis](https://azure.microsoft.com/en-in/products/cache/)
- [Azure Active Directory](https://azure.microsoft.com/en-us/products/active-directory/)
- [Azure Storage](https://azure.microsoft.com/en-us/products/category/storage/)

## Step 1
### App Service for Web Apps

IaaS options are great when you need access to the host machine to configure and deploy your app, but it leaves you with a lot of management overhead. Platform-as-a-Service (PaaS) takes care of that for you, simplifying deployment and updates, provided your application is supported in the PaaS environment. Azure has a few PaaS options - App Service is a very popular one.

In this lab you'll create an App Service deployment by pushing source code from your local machine. Azure will compile and configure the app for you.

### Reference

- [Azure App Service overview](https://docs.microsoft.com/en-us/azure/app-service/overview)

- [App Service Plan overview](https://docs.microsoft.com/en-us/azure/app-service/overview-hosting-plans)

- [`az appservice` commands](https://docs.microsoft.com/en-us/cli/azure/appservice?view=azure-cli-latest)

- [`az webapp` commands](https://docs.microsoft.com/en-us/cli/azure/webapp?view=azure-cli-latest)


### Explore App Service 

Create a new resource in the Portal - search for _Web app_ (which is one of the App Service types):

- the app needs a Resource Group and an App Service Plan
- you have options to publish from: source code, Docker containers, static web content
- for the source code option, you can choosse the runtime stack & OS (e.g. Java on Linux or .NET on Windows)

As usual, we won't create from the Portal, we'll switch to the CLI.

### Create an App Service Plan

Create a Resource Group for the lab:

```
az group create -n <your-name>-labs-appservice  -l eastus --tags courselabs=azure
```

Before we can create the app we need an App Service Plan - which is an abstraction of the infrastructure needed to run applications.

📋 Create an App Service Plan using the basic B1 SKU, and with one instance.


This is fairly straightforward: 

```
az appservice plan create -g <your-name>-labs-appservice -n <your-name>-app-service-01 --sku B1 --number-of-workers 1
```


Open the RG in the Portal. The only resource is the App Service Plan. Open that and you'll see an empty app list, and the scale up and scale out options (which are limited by the plan SKU).
	
### Create an app for deployment

We can create a Web App using the new App Service Plan. List the available runtimes to see what platforms are supported:

```
az webapp list-runtimes
```

Under the Windows options we have dotnet:6. This would work for pretty much any older .NET applications, and is a good fit for migrating apps to the cloud if you have the source code and you don't need the control you get with IaaS.

📋 Create web app in the service plan using the dotnet:6 runtime, and set for deployment from a local Git repository.


Check the help text for a new web app:

```
az webapp create --help
```

You need to specify the runtime, deployment method and a unique DNS name for the app:

```
az webapp create -g <your-name>-labs-appservice --plan <your-name>-app-service-01 --runtime 'dotnet:6' --name <dns-unique-app-name>
```


Check the RG again in the Portal when your CLI command has completed.

> Now the web app is listed as a separate resource - the type is _App Service_ - but you can navigate to the plan from the app and vice versa

Open the web app and you'll see it has a public URL, which uses the application name you set; HTTPS is provided by the platform. 

Browse to your app URL, you'll see a landing page saying "Your web app is running and waiting for your content".



## Step 2.
### Pre-Requisite - Provision an Azure SQL single database

```
subscription="b214611b-9a79-4e7e-afb0-3d9785737f10" # add subscription here

az account set -s $subscription # ...or use 'az login'
```
```
# Create a single database and configure a firewall rule
# Variable block
let "randomIdentifier=$RANDOM*$RANDOM"
location="East US"
resourceGroup="<your-name>-labs-appservice"
tag="create-and-configure-database"
server="sb-azuresql-server-$randomIdentifier"
database="sbazuresqldb$randomIdentifier"
login="azureuser"
password="Admin@1234567"
# Specify appropriate IP address values for your environment
# to limit access to the SQL Database server
startIp=0.0.0.0
endIp=255.255.255.255

echo "Using resource group $resourceGroup with login: $login, password: $password..."
echo "Creating $resourceGroup in $location..."
az group create --name $resourceGroup --location "$location" --tags $tag
echo "Creating $server in $location..."
az sql server create --name $server --resource-group $resourceGroup --location "$location" --admin-user $login --admin-password $password
echo "Configuring firewall..."
az sql server firewall-rule create --resource-group $resourceGroup --server $server -n AllowYourIp --start-ip-address $startIp --end-ip-address $endIp
echo "Creating $database on $server..."
az sql db create --resource-group $resourceGroup --server $server --name $database --sample-name AdventureWorksLT --edition Basic --capacity 5

```

1. Provision a Basic tier Azure Database instance on the Azure Portal
2. Create a New SQL Database Project to youre solution in Visual Studio IDE. Add two new folders - Scripts and Tables
3. Add a post-deployment scripts to the Scripts folder:

- [Scripts](https://github.com/Developing-Scalable-Apps-using-Azure/International-Bakers/tree/master/InternationalCookies.DataBase/Scripts)

4. Add the required tables to the Tables folder:

- [Tables](https://github.com/Developing-Scalable-Apps-using-Azure/International-Bakers/tree/master/InternationalCookies.DataBase/Tables)

4. Publish this project to your Azure SQL instance
5. Create application users and assign permissions

```
CREATE USER applicationUser with PASSWORD = 'App1234567'
GRANT select, insert, update, delete to applicationUser
```


## Step 3.
1. Create a new .NET 7 core MVC project 
2. Install the nuget packages by pasting the below snippet in <your-web-app-name>.csproj file


```
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="7.0.10" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="7.0.10">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="7.0.10" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="7.0.10">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.Extensions.Caching.Redis" Version="2.1.2" />
    <PackageReference Include="Microsoft.Extensions.Caching.StackExchangeRedis" Version="7.0.10" />
    <PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="7.0.9" />
  </ItemGroup>
  ```


3. Add an "images" folder in wwwroot. Upload - [cookies image](https://github.com/Developing-Scalable-Apps-using-Azure/International-Bakers/blob/master/InternationalBakers/wwwroot/images/cookie.jpg)

4. Update the home page to - [index.html](https://github.com/Developing-Scalable-Apps-using-Azure/International-Bakers/blob/master/InternationalBakers/Views/Home/Index.cshtml)
	
4. Scaffold the DB context - run the below command in the visual studio package manager console:
<img width="753" alt="image" src="https://user-images.githubusercontent.com/11691661/224944023-e2f3aa73-f45d-420d-82fa-c524eb320719.png">


```
Scaffold-DbContext "<your conn string>" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -ContextDir "Data" -DataAnnotations
```


5. Create new controllers using Entity Framework with models and views
<img width="678" alt="image" src="https://user-images.githubusercontent.com/11691661/224944171-aa3597ea-ac5f-41bd-8ced-f8182d4bf867.png">

<img width="952" alt="image" src="https://user-images.githubusercontent.com/11691661/224944344-7076a110-7dad-434b-99d1-8445e277911e.png">

Select the model (Cookie) and data context class from the dropdown

6. Dependency inject the connection string by replacing the OnConfiguring method with the below code snippet:

In DBContext.cs

```
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("ibdb");
            optionsBuilder.UseSqlServer(connectionString);
        }
```


In Program.cs


```
var connectionString = builder.Configuration.GetConnectionString("ibdb");
builder.Services.AddDbContext<<your-db-context-class>>(x => x.UseSqlServer(connectionString));
```


<img width="952" alt="image" src="https://user-images.githubusercontent.com/11691661/225085885-0e66713a-2ee9-4107-a6c8-9b18dba7c802.png">
	

In appsettings.json


```
"ConnectionStrings": {
"ibdb": "Server=tcp:sb-azuresql-server-286930812.database.windows.net,1433;Initial Catalog=<your-database-name>;Persist Security Info=False;User ID=azureuser;Password=Admin@1234567;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
```


<img width="1172" alt="image" src="https://user-images.githubusercontent.com/11691661/225085998-a861f9e6-a9c2-49c4-ad52-c1a36b9d6712.png">


## Step 4.

### Pre-requisite: Provision an Azure Redis cache instance
```
# Variable block
let "randomIdentifier=$RANDOM*$RANDOM"
location="East US"
resourceGroup="<your-rg-name>"
tag="create-manage-cache"
cache="ibcache-$randomIdentifier"
sku="basic"
size="C0"

# Create a resource group
echo "Creating $resourceGroup in "$location"..."
az group create --resource-group $resourceGroup --location "$location" --tags $tag

# Create a Basic C0 (256 MB) Redis Cache
echo "Creating $cache"
az redis create --name $cache --resource-group $resourceGroup --location "$location" --sku $sku --vm-size $size

# Get details of an Azure Cache for Redis
echo "Showing details of $cache"
az redis show --name "$cache" --resource-group $resourceGroup 

# Retrieve the hostname and ports for an Azure Redis Cache instance
redis=($(az redis show --name "$cache" --resource-group $resourceGroup --query [hostName,enableNonSslPort,port,sslPort] --output tsv))

# Retrieve the keys for an Azure Redis Cache instance
keys=($(az redis list-keys --name "$cache" --resource-group $resourceGroup --query [primaryKey,secondaryKey] --output tsv))

# Display the retrieved hostname, keys, and ports
echo "Hostname:" ${redis[0]}
echo "Non SSL Port:" ${redis[2]}
echo "Non SSL Port Enabled:" ${redis[1]}
echo "SSL Port:" ${redis[3]}
echo "Primary Key:" ${keys[0]}
echo "Secondary Key:" ${keys[1]}	
```
	
1. Add support for Azure Redis by pasting the below snippet in your project's csproj file
```
    <PackageReference Include="Microsoft.Extensions.Caching.Redis" Version="2.2.0" />
    <PackageReference Include="Microsoft.Extensions.Caching.StackExchangeRedis" Version="7.0.3" />
    <PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="6.0.12" />
``` 
3. Add the cache connection string to appsettings.json
4. Dependency inject the cache connection using Program.cs
```
builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration.GetConnectionString
        ("ibcache");
    option.InstanceName = "master";
});
```
5. Initialize _cache
```
private readonly IDistributedCache _cache;

        public CookiesController(sbazuresqldb286930812Context context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }
```
6. Update the CustomersController to fetch the list from redis
```
 public async Task<IActionResult> Index()
        {
            List<Cookie> cookies;
            var cachedCookies = _cache.GetString("cookieList");

            if (!string.IsNullOrEmpty(cachedCookies))
            {
                cookies = JsonConvert.DeserializeObject<List<Cookie>>(cachedCookies);
            }
            else
            {
                cookies = _context.Cookies.ToList();
                _cache.SetString("cookieList", JsonConvert.SerializeObject(cookies));
            }

            return View(cookies);
        }
```


## Step 5.
Create an Azure AD app registration with secret value: w5R8Q~yEPejsgL1aJ4ubYVa5v0.Y-jWOHHdXqcJC

1. In appsettings.json:
```
     "AzureAd": {
        "Instance": "https://login.microsoftonline.com/",
        "Domain": "datacouchtrainingoutlook.onmicrosoft.com",
        "TenantId": "87b7955d-4a78-474a-8a5c-6a5aaebe4ef2",
        "ClientId": "9c86c446-a50d-49ba-8108-e4d0364a8d44",
        "CallbackPath": "/signin-oidc"
      }
```

2. Add support for Azure AD:

```
 <PackageReference Include="Microsoft.Identity.Web" Version="1.1.0" />
 <PackageReference Include="Microsoft.Identity.Web.UI" Version="1.1.0" />
 <PackageReference Include="Microsoft.IdentityModel.Clients.ActiveDirectory" Version="5.2.9" />
```

3. Update Program.cs

Update the below builder services:

```
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddRazorPages()
     .AddMicrosoftIdentityUI();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = CookieSecurePolicy.Always;
});
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
```

In the configure method:
 
```
app.UseCookiePolicy();
app.UseAuthentication();
```     

## Step 6.
Add _LoginPartial.cshtml to the views -> shared directory

```
@using System.Security.Principal

<ul class="navbar-nav">
    @if (User.Identity.IsAuthenticated)
    {
        <li class="nav-item">
            <span class="navbar-text text-dark">Hello @User.Identity.Name!</span>
        </li>
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="MicrosoftIdentity" asp-controller="Account" asp-action="SignOut">Sign out</a>
        </li>
    }
    else
    {
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="MicrosoftIdentity" asp-controller="Account" asp-action="SignIn">Sign in</a>
        </li>
        <li><a asp-area="" asp-controller="Account" asp-action="Register">Register</a></li>

    }
</ul>
```

Add partial layout in the _Layout.cshtml file for sign in/out:

```
	<partial name="_LoginPartial" />
```

<img width="1172" alt="image" src="https://user-images.githubusercontent.com/11691661/225157130-5dcfb992-574a-4181-b014-000aad282ec1.png">


### Deploy the web app

Deploy the app using Visual Studio IDE. Right click on the web app in Visual Studio and click publish. Follow the authentication steps to deploy to the above app service.

## Notes
Sample scaffolding Commands - Do not run directly
```
dotnet ef dbcontext scaffold "Server=tcp:ibdbserver.database.windows.net,1433;Initial Catalog=ibdb;Persist Security Info=False;User ID=adminuser;Password=Admin1234567;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
Scaffold-DbContext "Server=tcp:ibdbserver.database.windows.net,1433;Initial Catalog=ibdb;Persist Security Info=False;User ID=adminuser;Password=Admin1234567;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
Scaffold-DbContext "Server=tcp:ibdbserver.database.windows.net,1433;Initial Catalog=ibdb;Persist Security Info=False;User ID=adminuser;Password=Admin1234567;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -ContextDir "Data" -DataAnnotations |
```
        
