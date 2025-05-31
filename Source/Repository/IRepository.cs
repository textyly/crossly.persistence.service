namespace Persistence.Repository
{
    public interface IRepository
    {
        Task<string[]> GetAll();
        Task<Stream?> GetById(string id);
        Task<string> Create(Stream dataModelStream);
        Task<bool> Replace(string id, Stream replacementDataModelStream);
        Task<bool> Rename(string id, string newName);
        Task<bool> Delete(string id);
    }
}