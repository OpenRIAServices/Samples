using OpenRiaServices.Hosting.AspNetCore;
using SimpleApplication.Web;

var builder = WebApplication.CreateBuilder(args);

// Register AddOpenRiaServices and all DomainServices
builder.Services.AddOpenRiaServices();
builder.Services.AddTransient<SampleDomainService>();

var app = builder.Build();
app.MapGet("/", () => "Hello Open Ria Services !\n\nNow you can start the client and call the Sample service");

// Enable mapping of all requests to root 
app.MapOpenRiaServices(builder =>
{
	builder.AddDomainService<SampleDomainService>();
});

app.Run();
