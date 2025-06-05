using Persistence.Handler;

namespace Persistence.Service
{
    public class RestService
    {
        private readonly string rootPath;
        private readonly string idPath;
        private readonly string renamePath;
        private readonly WebApplication webApp;
        private readonly IRequestHandler requestHandler;

        public RestService(WebApplication webApp, IRequestHandler requestHandler)
        {
            this.webApp = webApp;
            this.requestHandler = requestHandler;

            // TODO: implement HATEOS
            rootPath = "/api/v1/patterns";      // -> /api/v1/patterns
            idPath = $"{rootPath}/{{id}}";      // -> /api/v1/patterns/{id}
            renamePath = $"{idPath}/rename";    // -> /api/v1/patterns/{id}/rename
        }

        public void Run()
        {
            RegisterMethods();

            webApp.Run();
        }

        private void RegisterMethods()
        {
            webApp.MapGet(rootPath, requestHandler.GetAll);             // get all patterns     -> /api/v1/patterns
            webApp.MapGet(idPath, requestHandler.GetById);              // get a pattern        -> /api/v1/patterns/abc123
            webApp.MapPost(rootPath, (Delegate)requestHandler.Create);  // create a pattern     -> /api/v1/patterns
            webApp.MapPut(idPath, requestHandler.Replace);              // replace a pattern    -> /api/v1/patterns/abc123
            webApp.MapPatch(renamePath, requestHandler.Rename);         // rename a pattern     -> /api/v1/patterns/abc123/rename
            webApp.MapDelete(idPath, requestHandler.Delete);            // delete a pattern     -> /api/v1/patterns/abc123
        }
    }
}