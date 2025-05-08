using System.Text;
using System.Text.Json;
using System.IO.Compression;
using Persistence.DataModel;

namespace Persistence.Services
{
    public class RequestHandler : IRequestHandler
    {
        private readonly IPersistence persistence;
        private readonly JsonSerializerOptions serializerOptions;

        public RequestHandler(IPersistence persistence)
        {
            this.persistence = persistence;
            serializerOptions = new() { PropertyNameCaseInsensitive = true };
        }

        public async Task<Stream?> Get(string id)
        {
            CrosslyDataModel? dataModel = await persistence.Get(id);

            if (dataModel is null)
            {
                return null;
            }
            else
            {
                string jsonDataModel = JsonSerializer.Serialize(dataModel);

                MemoryStream memoryStream = new();
                using GZipStream gzip = new(memoryStream, CompressionLevel.Optimal, leaveOpen: true);
                using StreamWriter streamWriter = new(gzip, Encoding.UTF8);

                await streamWriter.WriteAsync(jsonDataModel);
                await streamWriter.FlushAsync();

                return memoryStream;
            }
        }

        public async Task<string> Save(Stream dataModelStream)
        {
            CrosslyDataModel? dataModel = await TryConvertToDataModel(dataModelStream);

            if (dataModel is null)
            {
                throw new InvalidDataException();
            }
            else
            {
                string id = await persistence.Save(dataModel);
                return id;
            }

        }

        private async Task<CrosslyDataModel?> TryConvertToDataModel(Stream dataModelStream)
        {
            using var gzipStream = new GZipStream(dataModelStream, CompressionMode.Decompress);

            CrosslyDataModel? dataModel = await JsonSerializer.DeserializeAsync<CrosslyDataModel>(gzipStream, serializerOptions);

            return dataModel;
        }
    }
}