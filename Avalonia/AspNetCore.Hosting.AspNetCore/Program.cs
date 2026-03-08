using OpenRiaServices.Hosting.AspNetCore;
using SimpleApplication.Web;

var builder = WebApplication.CreateBuilder(args);

// Register AddOpenRiaServices and all DomainServices
builder.Services.AddOpenRiaServices();
builder.Services.AddTransient<SampleDomainService>();
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		// ONLY for development, be more restrictive for production
		policy.AllowAnyMethod();
		policy.AllowAnyHeader();

		policy.WithOrigins("https://localhost:7169", "http://localhost:5235");
	});
});

var app = builder.Build();
app.UseCors();

app.MapGet("/", () => "Hello Open Ria Services !\n\nNow you can start the client and call the Sample service");

// Enable mapping of all requests to root 
app.MapOpenRiaServices(builder =>
{
	builder.AddDomainService<SampleDomainService>();
});

app.Run();
