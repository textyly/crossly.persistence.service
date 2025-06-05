using Persistence.Handler;

namespace Persistence.Service
{
    public class RestService(IRequestHandler requestHandler)
    {
        private const string rootPath = "/api/v1/patterns";      // -> /api/v1/patterns
        private const string idPath = $"{rootPath}/{{id}}";      // -> /api/v1/patterns/{id}
        private const string renamePath = $"{idPath}/rename";    // -> /api/v1/patterns/{id}/rename

        public void RegisterMethods(WebApplication app)
        {
            app.MapGet(rootPath, requestHandler.GetAll)
               .WithName("GetAllPatterns");

            app.MapGet(idPath, requestHandler.GetById)
               .WithName("GetPatternById");

            app.MapPost(rootPath, (Delegate)requestHandler.Create)
               .WithName("CreatePattern");

            app.MapPut(idPath, requestHandler.Replace)
               .WithName("ReplacePattern");

            app.MapPatch(renamePath, requestHandler.Rename)
               .WithName("RenamePattern");

            app.MapDelete(idPath, requestHandler.Delete)
               .WithName("DeletePattern");
        }
    }
}