using Persistence.Service;
using Persistence.Validation;
using Persistence.Conversion;
using Persistence.Persistence;
using Persistence.Compression;
using Persistence.Repository;
using Persistence.Requests;
using Persistence.Handler;

namespace Persistence
{
    public class RestApp(WebApplication webApp)
    {
        public async Task Run()
        {
            Validator validator = new();
            GZipCompressor compressor = new();
            MongoDbPersistence persistence = await CreatePersistence();
            DataModelRepository repository = new("", persistence); // TODO: !!!
            RequestFactory factory = new(validator, compressor, repository);
            RequestHandler handler = new(factory);

            RestService service = new(webApp, handler);
            service.RegisterMethods();

            webApp.Run();
        }

        private static async Task<MongoDbPersistence> CreatePersistence()
        {
            Converter converter = new();
            MongoDbPersistence persistence = new(converter);

            await persistence.Start();
            return persistence;
        }
    }
}