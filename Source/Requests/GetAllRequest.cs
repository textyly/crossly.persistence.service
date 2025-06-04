
using Persistence.Repository;

namespace Persistence.Requests
{
    public class GetAllRequest(IRepository repository) : IRequest
    {
        public async Task<IResult> Execute()
        {
            IResult result = await repository.GetAll();
            return result;
        }
    }
}