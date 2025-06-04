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

        public void RegisterMethods()
        {
            RegisterGetAll();    // -> /api/v1/patterns
            RegisterGetById();   // -> /api/v1/patterns/abc123
            RegisterCreate();    // -> /api/v1/patterns
            RegisterReplace();   // -> /api/v1/patterns/abc123
            RegisterRename();    // -> /api/v1/patterns/abc123/rename
            RegisterDelete();    // -> /api/v1/patterns/abc123
        }

        private void RegisterGetAll() => webApp.MapGet(rootPath, requestHandler.GetAll);
        private void RegisterGetById() => webApp.MapGet(idPath, requestHandler.GetById);
        private void RegisterCreate() => webApp.MapPost(rootPath, (Delegate)requestHandler.Create);
        private void RegisterReplace() => webApp.MapPut(idPath, requestHandler.Replace);
        private void RegisterRename() => webApp.MapPatch(renamePath, requestHandler.Rename);
        private void RegisterDelete() => webApp.MapDelete(idPath, requestHandler.Delete);
    }
}