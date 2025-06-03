using System.Text;
using System.Text.Json;
using System.IO.Compression;
using Persistence.DataModel;

namespace Persistence.Compression
{
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

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}