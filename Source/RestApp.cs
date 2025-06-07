using Persistence.Request;
using Persistence.Handler;
using Persistence.Service;
using Persistence.Repository;
using Persistence.Validation;
using Persistence.Conversion;
using Persistence.Compression;
using Persistence.Persistence;
using Persistence.HATEOS;

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

builder.Services.AddSingleton<IApiGenerator, ApiGenerator>();
builder.Services.AddSingleton<IConverter, Converter>();
builder.Services.AddSingleton<IValidator, Validator>();
builder.Services.AddSingleton<ICompressor, GZipCompressor>();
builder.Services.AddSingleton<IPersistence, MongoDbPersistence>();
builder.Services.AddSingleton<IRepository, Repository>();
builder.Services.AddSingleton<IRequestFactory, RequestFactory>();
builder.Services.AddSingleton<IRequestHandler, RequestHandler>();
builder.Services.AddSingleton<RestService>();

var app = builder.Build();

// Enable CORS globally for testing
app.UseCors();

var restService = app.Services.GetRequiredService<RestService>();
restService.RegisterMethods(app);

app.Run();
