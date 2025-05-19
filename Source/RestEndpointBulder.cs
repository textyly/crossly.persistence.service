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

var webbApp = builder.Build();

// Enable CORS globally for testing
webbApp.UseCors();

RestEndpointApp restEndpointApp = new(webbApp);
await restEndpointApp.Run();
