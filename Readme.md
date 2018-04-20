# ASP.NET Core Multitenancy Implementation

## How to run

```bash
cd src

dotnet clean && dotnet pack Multitenancy/ && dotnet build && dotnet run --project MultitenantAspApp
```

You can now check out 

http://localhost:5000/XX/api/ValuesWithDependencies

http://localhost:5000/SZ/api/ValuesWithDependencies

http://localhost:5000/XX/api/ValuesWithOptions

http://localhost:5000/SZ/api/ValuesWithOptions

Notice the different outputs per tenant.

## How to setup in your project

### Step 1: 
In your ASP.NET project, take a dependency on the Multitenancy package:

```bash
dotnet add package Multitenancy
```

and, after you setup all your dependencies, setup the overrides/individula dependencies for each of your tenants:

```cs
services.ConfigureTenant(t =>
{
    t.TenantId = "SZ";
    t.ServiceCollection.AddTransient<IHelloWorldService, SzHelloWorldService>();

    t.ServiceCollection.Configure<ValuesControllerOptions>(o =>
    {
        o.Value1Value = 42;
        o.Value2Value = "value1_configured_by_delegate_for_SZ";
    });
});
```

### Step 2: 
Add the middleware in the ```Configure``` method

```cs
app.UseMultitenancy();
```

### Step 3 (optional):
Add route prefix. 

If you wish to be able to pass the ```string tenantId```  parameter to your controller actions, you can add a route prefix that identifies the tenant and passes it on to your action methods.

To do this, in your ```Startup.cs``` file you have to change the Mvc registration in the ```ConfigureServices``` method from 

```cs
services.AddMvcCore();
```

to

```cs
services.AddMvcCore(o => { o.UseTenantRoutePrefix(); })
```

You can change the default name of ```tenantId``` by calling ```UseTenantRoutePrefix()``` with the desired name: ```UseTenantRoutePrefix("tenantIdentifier")```.

