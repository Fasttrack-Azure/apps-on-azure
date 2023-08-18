# This lab will help you create a .Net application using Azure PaaS services.

## Services used:

- [Azure App Services](https://azure.microsoft.com/en-us/products/app-service/)
- [Azure SQL](https://azure.microsoft.com/en-in/products/azure-sql/database/)
- [Azure Cache for Redis](https://azure.microsoft.com/en-in/products/cache/)
- [Azure Active Directory](https://azure.microsoft.com/en-us/products/active-directory/)
- [Azure Storage](https://azure.microsoft.com/en-us/products/category/storage/)

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
resourceGroup="Sid-RG-01"
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

## Step 1.
1. Provision a Basic tier Azure Database instance on the Azure Portal
2. Create a SQL database project 
3. Add the required tables and post-deployment scripts to this:

- [Scripts](https://github.com/Developing-Scalable-Apps-using-Azure/International-Bakers/tree/master/InternationalCookies.DataBase/Scripts)
- [Tables](https://github.com/Developing-Scalable-Apps-using-Azure/International-Bakers/tree/master/InternationalCookies.DataBase/Tables)

4. Publish this project to your Azure SQL instance
5. Create application users and assign permissions
```
CREATE USER applicationUser with PASSWORD = 'App1234567'
GRANT select, insert, update, delete to applicationUser
```

## Step 2.
1. Create a new .NET core MVC project 
2. Install the nuget packages
```
 <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="6.0.14" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="6.0.14">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="6.0.14">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>
  ```
3. Add an "images" folder in wwwroot. Upload - [cookies image](https://github.com/Developing-Scalable-Apps-using-Azure/International-Bakers/blob/master/InternationalBakers/wwwroot/images/cookie.jpg)
4. Scaffold the DB context - run the below command in the visual studio package manager console:
```
Scaffold-DbContext "<your conn string>" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -ContextDir "Data" -DataAnnotations
```
5. Create new controllers using Entity Framework with models and views
6. Dependency inject the connection string:

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
In appsettings.json
```
"ConnectionStrings": {
"ibdb": "Server=tcp:sb-azuresql-server-286930812.database.windows.net,1433;Initial Catalog=<your-database-name>;Persist Security Info=False;User ID=azureuser;Password=Admin@1234567;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
```

## Step 3.
1. Add support for Azure Redis
```
	  <PackageReference Include="Microsoft.Extensions.Caching.Redis" Version="2.2.0" />
	  <PackageReference Include="StackExchange.Redis" Version="2.2.88" />
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


## Step 4.
Create an Azure AD app registration
value
S3h8Q~d7p835hKkVQMMBh2sZ1LIkhGUm1Jpd6cst
ID
fc0ec619-e702-4c8a-9c7f-78ba3685a46f
1. In appsettings.json:
     "AzureAd": {
        "Instance": "https://login.microsoftonline.com/",
        "Domain": "siddharthdwngmail.onmicrosoft.com",
        "TenantId": "0e97f85a-b424-40f3-8fcc-bdde6023332e",
        "ClientId": "8fa7b6f3-3378-43b0-ad32-2dcffb3131e2",
        "CallbackPath": "/signin-oidc"
      }

2. Add support for Azure AD
```
 <PackageReference Include="Microsoft.Identity.Web" Version="1.1.0" />
 <PackageReference Include="Microsoft.Identity.Web.UI" Version="1.1.0" />
 <PackageReference Include="Microsoft.IdentityModel.Clients.ActiveDirectory" Version="5.2.9" />
 ```
3. Update startup.cs
In the ConfigureServices method:
```
services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddRazorPages()
                 .AddMicrosoftIdentityUI();
                 
services.Configure<CookiePolicyOptions>(options =>
            {
                options.Secure = CookieSecurePolicy.Always;
            });
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));
```         
In the configure method:
 
```
app.UseCookiePolicy();
app.UseAuthentication();
```                
## Step 5.
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

	

Sample scaffolding Commands - Do not run directly
```
dotnet ef dbcontext scaffold "Server=tcp:ibdbserver.database.windows.net,1433;Initial Catalog=ibdb;Persist Security Info=False;User ID=adminuser;Password=Admin1234567;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
Scaffold-DbContext "Server=tcp:ibdbserver.database.windows.net,1433;Initial Catalog=ibdb;Persist Security Info=False;User ID=adminuser;Password=Admin1234567;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
Scaffold-DbContext "Server=tcp:ibdbserver.database.windows.net,1433;Initial Catalog=ibdb;Persist Security Info=False;User ID=adminuser;Password=Admin1234567;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -ContextDir "Data" -DataAnnotations |
```
        
