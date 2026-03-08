# AspNetCore

This examples shows how one can use OpenRiaServices with a AspNetCore server and a Net 6.0 client.

To run/debug this project:
1. Compile all projects.
2. Setup AspNetCore.Client and AspNetCore.Hosting.AspNetCore as startup projects in Visual Studio.
3. Run

## Server Setup

1. Create a AspNetCore project (preferably somewhat empty)
1. Install `OpenRiaServices.Hosting.AspNetCore" and `OpenRiaServices.Server` nuget
1. Setup OpenRiaServices
    1. Register OpenRiaServices by calling `Services.AddOpenRiaServices();`
    2. Register all DomainServices manually (as Transient)
    3. Add call to Map OpenRiaServices to a specific path (or skip prefix to map all requests to the root)

    ```
    // Enable mapping of all requests to root 
    app.MapOpenRiaServices(builder =>
    {
        builder.AddDomainService<SampleDomainService>();
    });
    ```

1. Àdd one or more DomainServices


## Client Setup


Setup `DomainContext.DomainClientFactory` before creating your first DomainContext.


See `App.OnFrameworkInitializationCompleted`setup DomainContext.DomainClientFactory

```csharp
public override void OnFrameworkInitializationCompleted()
{
	DomainContext.DomainClientFactory = new OpenRiaServices.Client.DomainClients.BinaryHttpDomainClientFactory(new Uri("https://localhost:50694/", UriKind.Absolute), new System.Net.Http.HttpClientHandler());

```

## Avalonia usage vs WPF

1. You cannot use bind directly to `EnntitySet` such as `ctx.YourEntityCollection`, instead wrap the EntitySet in a `EntityList<T>`