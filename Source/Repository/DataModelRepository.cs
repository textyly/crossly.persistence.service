using Persistence.DataModel;
using Persistence.Persistence;
using Persistence.Compression;

namespace Persistence.Repository
{
    public class DataModelsRepository(IPersistence persistence, ICompressor compressor) : IRepository
    {
        private readonly IPersistence persistence = persistence;
        private readonly ICompressor compressor = compressor;

        public async Task<Stream?> Get(string id)
        {
            CrosslyDataModel? dataModel = await persistence.Get(id);

            return dataModel is null
                ? null
                : await compressor.CompressToStream(dataModel);
        }

        public async Task<string> Save(Stream dataModelStream)
        {
            CrosslyDataModel? dataModel = await compressor.TryDecompressToDataModel(dataModelStream);

            return dataModel is null
                ? throw new InvalidDataException()
                : await persistence.Save(dataModel);
        }
    }
}