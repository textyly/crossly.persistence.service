using Persistence.DataModel;

namespace Persistence.Persistence
{
    public interface IPersistence
    {
        Task Start();
        Task<string[]> GetAll();
        Task<CrosslyDataModel?> GetById(string id);
        Task<string> Create(CrosslyDataModel dataModel);
        Task<bool> Replace(string id, CrosslyDataModel newDataModel);
        Task<bool> Rename(string id, string newName);
        Task<bool> Delete(string id);
    }
}