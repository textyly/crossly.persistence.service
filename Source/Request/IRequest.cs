namespace Persistence.Request
{
    public interface IRequest
    {
        Task<IResult> Execute();
    }
}