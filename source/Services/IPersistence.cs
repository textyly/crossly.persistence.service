using Persistence.DataModel;

namespace Persistence.Services
{
    public interface IPersistence
    {
        Task<string> Save(CrosslyDataModel dataModel);

        Task<CrosslyDataModel?> Get(string id);
    }
}