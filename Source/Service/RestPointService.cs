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
            webApp.MapGet("/get/by-id", GetByIdDataModel);
            webApp.MapGet("/get/by-name", GetByNameDataModel);
            webApp.MapGet("/get/all", GetAll);

            // TODO: check how to use a method, not a lambda
            webApp.MapPost("/save", async (HttpContext httpContext) =>
            {
                Stream body = httpContext.Request.Body;
                string id = await repository.Save(body);

                return Results.Ok(new { id });
            });

            webApp.Run();
        }

        private async Task<IResult> GetByIdDataModel(string id)
        {
            Stream? stream = await repository.GetById(id);
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

        private async Task<IResult> GetByNameDataModel(string name)
        {
            Stream? stream = await repository.GetByName(name);
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

        private async Task<IResult> GetAll()
        {
            string[] ids = await repository.GetAll();
            return Results.Ok(new { ids });
        }
    }
}