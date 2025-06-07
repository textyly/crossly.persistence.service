namespace Persistence.Handler
{
    public interface IRequestHandler
    {
        Task<IResult> GetById(string id);
        Task<IResult> GetAll();
        Task<IResult> Create(HttpContext httpContext);
        Task<IResult> Replace(string id, HttpContext httpContext);
        Task<IResult> Rename(string id, RenameRequestInput request);
        Task<IResult> Delete(string id);
    }
}