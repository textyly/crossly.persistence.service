namespace Persistence.Services
{
    public interface IRequestHandler
    {
        Task<string> Save(Stream dataModelStream);
        Task<Stream?> Get(string id);
    }
}