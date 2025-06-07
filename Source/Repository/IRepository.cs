namespace Persistence.Repository
{
    public interface IRepository
    {
        Task<IResult> GetAll();
        Task<IResult> GetById(string id);
        Task<IResult> Create(Stream dataModelStream);
        Task<IResult> Replace(string id, Stream dataModelStream);
        Task<IResult> Rename(string id, string newName);
        Task<IResult> Delete(string id);
    }
}