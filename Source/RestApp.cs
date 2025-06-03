using Persistence.Service;
using Persistence.Validation;
using Persistence.Conversion;
using Persistence.Persistence;
using Persistence.Compression;

namespace Persistence
{
    public class RestApp(WebApplication webApp)
    {
        private readonly WebApplication webApp = webApp;

        public async Task Run()
        {
            Validator validator = new();
            GZipCompressor compressor = new();
            MongoDbPersistence persistence = await CreatePersistence();

            RestService service = new(webApp, validator, compressor, persistence);
            service.RegisterMethods();
            webApp.Run();
        }

        private async Task<MongoDbPersistence> CreatePersistence()
        {
            Converter converter = new();
            MongoDbPersistence persistence = new(converter);

            await persistence.Start();
            return persistence;
        }
    }
}