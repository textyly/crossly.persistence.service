using Persistence.DataModel;

namespace Persistence.Repository
{
    public interface IRepository
    {
        Task<IResult> GetAll();
        Task<CrosslyDataModel?> GetById(string id);
        Task<IResult> Create(CrosslyDataModel dataModel);
        Task<IResult> Replace(string id, CrosslyDataModel dataModel);
        Task<IResult> Rename(string id, string newName);
        Task<IResult> Delete(string id);
    }
}