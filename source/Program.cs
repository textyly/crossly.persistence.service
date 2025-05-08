using Persistence.Services;

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

PersistentStorage persistence = new();
RequestHandler handler = new(persistence);

app.MapGet("/get", async (string id) =>
{
    Stream? stream = await handler.Get(id);

    stream!.Position = 0; // rewind stream

    return stream is null
        ? Results.NotFound()
        : Results.File(stream, contentType: "application/octet-stream", fileDownloadName: null);
});

app.MapPost("/save", async (HttpContext http) =>
{
    Stream body = http.Request.Body;
    string id = await handler.Save(body);

    return Results.Ok(new { id });
});

app.Run();
