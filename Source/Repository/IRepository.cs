namespace Persistence.Repository
{
    public interface IRepository
    {
        Task<string[]> GetAll();
        Task<Stream?> GetById(string id);
        Task<Stream?> GetByName(string name);
        Task<string> Save(Stream dataModelStream);
        Task<bool> Replace(string id, Stream replacementDataModelStream);
        Task<bool> Rename(string id, string newName);
    }
}