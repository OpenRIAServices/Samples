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

1. For WPF project make sure to create a separate project for code genereration
2. Install `OpenRiaServices.Client.Core` and `OpenRiaServices.Client.CodeGen` (at least 5.4.0-preview) nuget s
2. Add `<LinkedOpenRiaServerProject>` tag to csproj 
3. At program startup before the forst call to the server the "DomainClientFactory" and server Uri needs to be specified
 ```
   var serverUri = new Uri("YOUR URI", UriKind.Absolute);
   DomainContext.DomainClientFactory = new OpenRiaServices.Client.DomainClients.BinaryHttpDomainClientFactory(serverUri , new System.Net.Http.HttpClientHandler());
```
4. You can now create instances of your client side context (Samples
```
var ctx = new SampleDomainContext();

var resultFromInvoke = await ctx.AddOneAsync(22);
Console.WriteLine("22 plus one is {resultFromInvoke.Value}");
```

### With 5.4.0 rherer is no need to use .Net Framework project to get working code generation for AspNet.Core

Se the following in "CodeGen.csproj"

```
<LinkedOpenRiaServerProject>..\AspNetCore.Hosting.AspNetCore\AspNetCore.Hosting.AspNetCore.csproj</LinkedOpenRiaServerProject>
```

### Avoid build warning about duplicate includes

Remove the generated file once and then add it as "none" to still see it in solution view
```
  <ItemGroup>
    <Compile Remove="Generated_Code\AspNetCore.Hosting.AspNetCore.g.cs" />
    <None Include="Generated_Code\AspNetCore.Hosting.AspNetCore.g.cs" />
  </ItemGroup>
```

