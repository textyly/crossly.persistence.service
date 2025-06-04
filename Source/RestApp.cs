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
            IPersistence persistence = CreatePersistence();
            await persistence.Start();

            IRequestHandler handler = CreateRequestHandler(persistence);
            RestService service = new(webApp, handler);

            service.Run();
        }

        private static IRequestHandler CreateRequestHandler(IPersistence persistence)
        {
            Validator validator = new();
            GZipCompressor compressor = new();
            DataModelRepository repository = new("", persistence); // TODO: !!!

            RequestFactory factory = new(validator, compressor, repository);
            RequestHandler handler = new(factory);

            return handler;
        }

        private static IPersistence CreatePersistence()
        {
            Converter converter = new();
            MongoDbPersistence persistence = new(converter);
            return persistence;
        }
    }
}