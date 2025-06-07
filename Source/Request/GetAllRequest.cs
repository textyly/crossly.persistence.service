using Persistence.Repository;
using Persistence.Validation;

namespace Persistence.Request
{
#pragma warning disable CS9113 // Parameter is unread.
    public class GetAllRequest(GetAllInput input, IRepository repository, IValidator validator) : RequestBase<GetAllInput>(input)
#pragma warning restore CS9113 // Parameter is unread.
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