using Persistence;

var builder = WebApplication.CreateSlimBuilder(args);

// Register CORS for testing
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var webApp = builder.Build();

// Enable CORS globally for testing
webApp.UseCors();

RestApp restApp = new(webApp);
await restApp.Run();
