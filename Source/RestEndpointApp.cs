using Persistence.Service;
using Persistence.Repository;
using Persistence.Conversion;
using Persistence.Persistence;

namespace Persistence
{
    public class RestEndpointApp(WebApplication webApp)
    {
        private readonly WebApplication webApp = webApp;
        private RestEndpointService service;

        public async Task Run()
        {   
            MongoDbPersistence persistence = new();
            await persistence.Start();

            GZipConverter converter = new();
            DataModelRepository repository = new(persistence, converter);

            service = new RestEndpointService(webApp, repository);
            service.Run();
        }
    }
}