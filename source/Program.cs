using System.IO.Compression;
using System.Text.Json;
using Persistence.DataModel;

var builder = WebApplication.CreateSlimBuilder(args);
// Register CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()  // or specify your front-end URL like .WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();  // Allow any HTTP method
    });
});

var app = builder.Build();

// Enable CORS globally
app.UseCors();

app.MapGet("/", () => "Hello World!");

app.MapPost("/save", static async (HttpContext http) =>
{
    using var gzipStream = new GZipStream(http.Request.Body, CompressionMode.Decompress);

    var options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true // Enable case-insensitive matching
    };

    CrosslyDataModel? dataModel = await JsonSerializer.DeserializeAsync<CrosslyDataModel>(gzipStream, options);

    if (dataModel is null)
    {
        return Results.BadRequest("Invalid Data Model");
    }
    else
    {
        Console.WriteLine($"Saving pattern for Name: {dataModel.Name}");

        return Results.Ok(new { success = true });
    }
});

app.Run();
