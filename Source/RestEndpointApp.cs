using Persistence.Service;
using Persistence.Repository;
using Persistence.Conversion;
using Persistence.Persistence;

namespace Persistence
{
    public class RestEndpointApp
    {
        private readonly RestEndpointService service;

        public RestEndpointApp(WebApplication webApp)
        {
            GZipConverter converter = new();
            MongoDbPersistence persistence = new();
            DataModelRepository repository = new(persistence, converter);

            service = new RestEndpointService(webApp, repository);
        }

        public void Run()
        {
            service.Run();
        }
    }
}