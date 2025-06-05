using Persistence.Handler;
using Persistence.Service;
using Persistence.Request;
using Persistence.Validation;
using Persistence.Conversion;
using Persistence.Persistence;
using Persistence.Compression;

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

            string createdUri = $"{"???"}/{{0}}";   // TODO: !!! // -> /api/v1/patterns/{0}
            Repository.Repository repository = new(createdUri, persistence, validator, compressor);

            RequestFactory factory = new(validator, repository);
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