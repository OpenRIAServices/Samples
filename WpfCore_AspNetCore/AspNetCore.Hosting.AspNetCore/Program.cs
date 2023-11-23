using AspNetCore.Hosting.AspNetCore.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using OpenRiaServices.Hosting.AspNetCore;
using SimpleApplication.Web;

var builder = WebApplication.CreateBuilder(args);

// Register AddOpenRiaServices and all DomainServices
builder.Services.AddOpenRiaServices();
builder.Services.AddTransient<SampleDomainService>();
builder.Services.AddTransient<MyAuthenticationService>();

// Additional dependencies for cookie based AuthenticationService
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services.AddHttpContextAccessor();
// Add default authorization policy which requires user to be authenticated
builder.Services.AddAuthorization();

var app = builder.Build();

// Add authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello Open Ria Services !\n\nNow you can start the client and call the Sample service");

// Enable mapping of all requests to root 
app.MapOpenRiaServices(builder =>
{
    builder.AddDomainService<SampleDomainService>()
        .AllowAnonymous(); // RequiresAuthentication is used on the class instead
    builder.AddDomainService<MyAuthenticationService>();
}).RequireAuthorization();

app.Run();
