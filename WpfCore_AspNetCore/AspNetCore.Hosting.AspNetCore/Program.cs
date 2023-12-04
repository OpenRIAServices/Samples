using System.Net;
using AspNetCore.Hosting.AspNetCore.Services;
using OpenRiaServices.Hosting.AspNetCore;
using SimpleApplication.Web;

var builder = WebApplication.CreateBuilder(args);

// Register AddOpenRiaServices and all DomainServices
builder.Services.AddOpenRiaServices();
builder.Services.AddTransient<SampleDomainService>();
builder.Services.AddTransient<MyAuthenticationService>();

// Additional dependencies for cookie based AuthenticationService
builder.Services.AddAuthentication()
    .AddCookie(opt =>
    {
        // Example of how to set status code when trying to access protected
        // Without this you can expect http 404  unless you have a matching endpoint for the RedirectUri ("/Account/Login")
        opt.Events.OnRedirectToLogin = (ctx) =>
         {
             if (ctx.Response.StatusCode == 200)
             {
                 ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                 ctx.Response.Headers.Location = ctx.RedirectUri;
             }

             return Task.CompletedTask;
         };
    });
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
