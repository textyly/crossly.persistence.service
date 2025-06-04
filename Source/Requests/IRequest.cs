namespace Persistence.Requests
{
    public interface IRequest
    {
        Task<IResult> Execute();
    }
}