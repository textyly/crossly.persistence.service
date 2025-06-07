using Persistence.Repository;
using Persistence.Validation;

namespace Persistence.Request
{
    public class ReplaceRequest(ReplaceInput input, IRepository repository, IValidator validator) : RequestBase<ReplaceInput>(input)
    {
        protected override IResult? Validate(ReplaceInput input)
        {
            if (!validator.IsValidId(input.Id))
            {
                return Results.BadRequest($"{nameof(input.Id)} is not valid.");
            }

            if (!validator.IsValidStream(input.DataModelStream))
            {
                return Results.BadRequest($"data model is not valid.");
            }

            return default;
        }

        protected override async Task<IResult> ExecuteRequest(ReplaceInput input)
        {
            IResult result = await repository.Replace(input.Id, input.DataModelStream);
            return result;
        }
    }

    public record ReplaceInput(string Id, Stream DataModelStream);
}