using Persistence.Repository;
using Persistence.Validation;

namespace Persistence.Request
{
    public class GetByIdRequest(GetByIdInput input, IRepository repository, IValidator validator) : RequestBase<GetByIdInput>(input)
    {
        protected override IResult? Validate(GetByIdInput input)
        {
            if (!validator.IsValidId(input.Id))
            {
                return Results.BadRequest($"{nameof(input.Id)} is not valid.");
            }

            return default;
        }

        protected override async Task<IResult> ExecuteRequest(GetByIdInput input)
        {
            IResult result = await repository.GetById(input.Id);
            return result;
        }
    }

    public record GetByIdInput(string Id);
}