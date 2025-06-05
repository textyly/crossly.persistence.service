using Persistence.Repository;
using Persistence.Validation;

namespace Persistence.Request
{
    public class DeleteRequest(DeleteInput input, IRepository repository, IValidator validator) : RequestBase<DeleteInput>(input)
    {
        protected override IResult? Validate(DeleteInput input)
        {
            if (!validator.IsValidId(input.Id))
            {
                return Results.BadRequest($"{nameof(input.Id)} is not valid.");
            }

            return default;
        }

        protected override async Task<IResult> ExecuteRequest(DeleteInput input)
        {
            IResult result = await repository.Delete(input.Id);
            return result;
        }
    }

    public record DeleteInput(string Id);
}