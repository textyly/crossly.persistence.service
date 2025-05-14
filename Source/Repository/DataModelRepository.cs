using Persistence.DataModel;
using Persistence.Conversion;
using Persistence.Persistence;

namespace Persistence.Repository
{
    public class DataModelRepository(IPersistence persistence, IConverter converter) : IRepository
    {
        private readonly IPersistence persistence = persistence;
        private readonly IConverter converter = converter;

        public async Task<Stream?> Get(string id)
        {
            CrosslyDataModel? dataModel = await persistence.Get(id);

            return dataModel is null
                ? null
                : await converter.ConvertToStream(dataModel);
        }

        public async Task<string> Save(Stream dataModelStream)
        {
            CrosslyDataModel? dataModel = await converter.TryConvertToDataModel(dataModelStream);

            return dataModel is null
                ? throw new InvalidDataException()
                : await persistence.Save(dataModel);
        }
    }
}