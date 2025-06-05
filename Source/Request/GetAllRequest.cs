using Persistence.Repository;

namespace Persistence.Request
{
    public class GetAllRequest(GetAllInput input, IRepository repository) : RequestBase<GetAllInput>(input)
    {
        protected override IResult? Validate(GetAllInput input) => default;

        protected override async Task<IResult> ExecuteRequest(GetAllInput input)
        {
            IResult result = await repository.GetAll();
            return result;
        }
    }

    public record GetAllInput();
}