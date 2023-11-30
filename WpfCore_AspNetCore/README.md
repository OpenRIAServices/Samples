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

    ```csharp
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
```csharp
   var serverUri = new Uri("YOUR URI", UriKind.Absolute);
   DomainContext.DomainClientFactory = new OpenRiaServices.Client.DomainClients.BinaryHttpDomainClientFactory(serverUri , new System.Net.Http.HttpClientHandler());
```
4. You can now create instances of your client side context (Samples
```csharp
var ctx = new SampleDomainContext();

var resultFromInvoke = await ctx.AddOneAsync(22);
Console.WriteLine("22 plus one is {resultFromInvoke.Value}");
```

### With 5.4.0 rherer is no need to use .Net Framework project to get working code generation for AspNet.Core

Se the following in "CodeGen.csproj"

```xml
<LinkedOpenRiaServerProject>..\AspNetCore.Hosting.AspNetCore\AspNetCore.Hosting.AspNetCore.csproj</LinkedOpenRiaServerProject>
```

### Avoid build warning about duplicate includes

Remove the generated file once and then add it as "none" to still see it in solution view
```xml
  <ItemGroup>
    <Compile Remove="Generated_Code\AspNetCore.Hosting.AspNetCore.g.cs" />
    <None Include="Generated_Code\AspNetCore.Hosting.AspNetCore.g.cs" />
  </ItemGroup>
```



# Authentication and Authorization
**This sample** shows how cookie based login similar to ASP.NET Membership provider *can* be handled.
You will need to tweak it so it validates credentials, assign correct Claims (Roles) to users and fits your choosen scheme for authentication.

On the server you need to setup Authentication (and preferably Authorization) using "normal" aspnetcore practices.
 The official [AspNetCore cookie documentation](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-7.0)  contains login as well as logout code.
More details can be found inhttps://github.com/OpenRIAServices/OpenRiaServices/issues/445


### Client setup
For the client, **you need to ensure that all HttpClients share the same CookieContainer** and that the it is set to use to cookies (https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclienthandler.usecookies?view=net-7.0#system-net-http-httpclienthandler-usecookies)

### Asp.Net Core Setup Authentication and Authorization setup

In Program.cs the following code is needed

Service registrations, adds cookie based authentication and enable Authorization
```csharp
// Additional dependencies for cookie based AuthenticationService
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization();
```

After the "application has been built" and the middleware is configured you should add Authentication (and Authorization if used).
They should be added **before** *OpenRiaServices*

```csharp
// Add authentication
app.UseAuthentication();
app.UseAuthorization();

// Enable OpenRiaServices
app.MapOpenRiaServices(....
```

### Protecting DomainServices using Authorization middleware

A good idea is to protect whole DomainServices, or all of them, by requiring authorization.
The [Authorization middleware](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/introduction) should run before the DomainService is created so you will avoid any cost with creating the DomainService and it's dependencies, providing a much better protection against DOS.

**IMPORANT**: error message on the client will be different and not as user friendly as if the `[RequiresAuthentication]` attribute is used.
You might want to consider changing the HTTP status code to 403 instead of 404 for a better experience (https://learn.microsoft.com/en-us/aspnet/core/security/authorization/customizingauthorizationmiddlewareresponse)
**Note**: If you need to selectivly only require authentication for some Submit operations (Insert, Update, Delete or Entity Action) then remember to use `[RequiresAuthentication]`

**Require request to all DomainServices be authorized using default authorization policy**:

```csharp
app.MapOpenRiaServices(builder =>
{
    builder.AddDomainService<SampleDomainService>();
    builder.AddDomainService<MyAuthenticationService>();
}).RequireAuthorization();
```

You can also control the setting per DomainService using code
```csharp
app.MapOpenRiaServices(builder =>
{
    builder.AddDomainService<SampleDomainService>()
        .RequireAuthorization();
    builder.AddDomainService<MyAuthenticationService>();
});
```

You can also control the setting per DomainService using attributes
```csharp
[Authorize]
public class MyAuthenticationService : DomainService, IAuthentication<MyUser>
{
    [AllowAnonymous]
    public MyUser GetUser() {...}
```
