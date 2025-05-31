using Persistence.Repository;

namespace Persistence.Service
{
    // TODO: 1) this service will sit in front of a database 
    // and will not be reached directly from the UI. 
    // Probably it will be invoked by another service which is going to provide 
    // validated and well formatted data models 

    // TODO: 2) start using grpc instead of rest since an internal communication
    // between services will take place

    public class RestEndpointService(WebApplication webApp, IRepository repository)
    {
        private readonly WebApplication webApp = webApp;
        private readonly IRepository repository = repository;

        public void Run()
        {
            webApp.MapGet("/api/patterns", GetAll);
            webApp.MapGet("/api/patterns/{id}", GetById);

            // TODO: check how to use a method, not a lambda
            webApp.MapPost("/api/patterns", async (HttpContext httpContext) =>
            {
                Stream body = httpContext.Request.Body;
                string id = await repository.Create(body);

                return Results.Ok(new { id });
            });

            webApp.MapDelete("/api/patterns/{id}", Delete);

            webApp.MapPut("/api/patterns/{id}", async (string id, HttpContext httpContext) =>
            {
                Stream body = httpContext.Request.Body;
                bool success = await repository.Replace(id, body);

                return Results.Ok(new { success });
            });

            webApp.MapPatch("/api/patterns/{id}/rename", Rename);


            webApp.Run();
        }

        private async Task<IResult> GetById(string id)
        {
            Stream? stream = await repository.GetById(id);
            if (stream is null)
            {
                return Results.NotFound();
            }
            else
            {
                stream!.Position = 0;
                return Results.File(stream, contentType: "application/octet-stream", fileDownloadName: default);
            }
        }

        private async Task<IResult> GetAll()
        {
            string[] ids = await repository.GetAll();
            return Results.Ok(new { ids });
        }

        private async Task<IResult> Rename(string id, RenamePatternRequest request)
        {
            bool success = await repository.Rename(id, request.NewName);
            return Results.Ok(new { success });
        }

        private async Task<IResult> Delete(string id)
        {
            // TODO:
            return Results.NotFound();
        }
    }

    public record RenamePatternRequest(string NewName);
}