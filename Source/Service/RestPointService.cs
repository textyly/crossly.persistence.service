using Persistence.Repository;

namespace Persistence.Service
{
    public class RestEndpointService(WebApplication webApp, IRepository repository)
    {
        private readonly WebApplication webApp = webApp;
        private readonly IRepository repository = repository;

        public void Run()
        {
            webApp.MapGet("/get", GetDataModel);

            // TODO: check how to use a method, not a lambda
            webApp.MapPost("/save", async (HttpContext httpContext) =>
            {
                Stream body = httpContext.Request.Body;
                string id = await repository.Save(body);

                return Results.Ok(new { id });
            });

            webApp.Run();
        }

        private async Task<IResult> GetDataModel(string id)
        {
            Stream? stream = await repository.Get(id);
            if (stream is null)
            {
                return Results.NotFound();
            }
            else
            {
                stream!.Position = 0;
                return Results.File(stream, contentType: "application/octet-stream", fileDownloadName: null);
            }
        }
    }
}