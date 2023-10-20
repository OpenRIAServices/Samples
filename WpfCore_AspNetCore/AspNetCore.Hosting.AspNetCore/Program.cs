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

var app = builder.Build();

// Add authentication
app.UseAuthentication();
// TODO: app.UseAuthorization();

app.MapGet("/", () => "Hello Open Ria Services !\n\nNow you can start the client and call the Sample service");

// Enable mapping of all requests to root 
app.MapOpenRiaServices(builder =>
{
	builder.AddDomainService<SampleDomainService>();
    builder.AddDomainService<MyAuthenticationService>();
});

app.Run();
