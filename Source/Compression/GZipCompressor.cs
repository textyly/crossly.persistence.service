using System.Text;
using System.Text.Json;
using System.IO.Compression;
using Persistence.DataModel;

namespace Persistence.Compression
{
    // TODO: indexesX and indexesY (pattern) will come in validated gzip format and compression will not be needed for them
    // they will be saved into the database as they are, in gzip format in order to save space and CPU
    public class GZipCompressor : ICompressor
    {
        private readonly JsonSerializerOptions serializerOptions;

        public GZipCompressor()
        {
            serializerOptions = JsonSerializerOptions.Web;
        }

        public async Task<CrosslyDataModel?> TryDecompressToDataModel(Stream dataModelStream)
        {
            using var gzipStream = new GZipStream(dataModelStream, CompressionMode.Decompress);

            CrosslyDataModel? dataModel = await JsonSerializer.DeserializeAsync<CrosslyDataModel>(gzipStream, serializerOptions);

            return dataModel;
        }

        public async Task<Stream> CompressToStream(CrosslyDataModel dataModel)
        {
            string jsonDataModel = JsonSerializer.Serialize(dataModel, serializerOptions);

            // TODO: check for any kind of leaks
            MemoryStream memoryStream = new();
            using GZipStream gzip = new(memoryStream, CompressionLevel.Optimal, leaveOpen: true);
            using StreamWriter streamWriter = new(gzip, Encoding.UTF8);

            await streamWriter.WriteAsync(jsonDataModel);
            await streamWriter.FlushAsync();

            return memoryStream;
        }
    }
}