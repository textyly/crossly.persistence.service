using Persistence.DataModel;
using Persistence.Persistence;
using Persistence.Compression;

namespace Persistence.Repository
{
    public class DataModelsRepository(IPersistence persistence, ICompressor compressor) : IRepository
    {
        private readonly IPersistence persistence = persistence;
        private readonly ICompressor compressor = compressor;

        public async Task<string[]> GetAll()
        {
            return await persistence.GetAll();
        }

        public async Task<Stream?> GetById(string id)
        {
            CrosslyDataModel? dataModel = await persistence.GetById(id);

            return dataModel is null
                ? default
                : await compressor.CompressToStream(dataModel);
        }

        public async Task<Stream?> GetByName(string name)
        {
            CrosslyDataModel? dataModel = await persistence.GetByName(name);

            return dataModel is null
                ? default
                : await compressor.CompressToStream(dataModel);
        }

        public async Task<string> Save(Stream dataModelStream)
        {
            CrosslyDataModel? dataModel = await compressor.TryDecompressToDataModel(dataModelStream);

            return dataModel is null
                ? throw new InvalidDataException()
                : await persistence.Save(dataModel);
        }

        public async Task<bool> Replace(string id, Stream replacementDataModelStream)
        {
            CrosslyDataModel? replacementDataModel = await compressor.TryDecompressToDataModel(replacementDataModelStream);

            return replacementDataModel is null
                ? throw new InvalidDataException()
                : await persistence.Replace(id, replacementDataModel);
        }

        public Task<bool> Rename(string id, string newName)
        {
            return persistence.Rename(id, newName);
        }
    }
}