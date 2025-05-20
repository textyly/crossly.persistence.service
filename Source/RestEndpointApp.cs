using Persistence.Service;
using Persistence.Repository;
using Persistence.Conversion;
using Persistence.Persistence;
using Persistence.Compression;

namespace Persistence
{
    public class RestEndpointApp(WebApplication webApp)
    {
        private readonly WebApplication webApp = webApp;

        public async Task Run()
        {
            IPersistence persistence = await CreatePersistence();
            IRepository repository = CreateRepository(persistence);

            RestEndpointService service = new(webApp, repository);
            service.Run();
        }

        private async Task<IPersistence> CreatePersistence()
        {
            Converter converter = new();
            MongoDbPersistence persistence = new(converter);

            await persistence.Start();
            return persistence;
        }

        private IRepository CreateRepository(IPersistence persistence)
        {
            GZipCompressor compressor = new();
            DataModelsRepository repository = new(persistence, compressor);

            return repository;
        }
    }
}