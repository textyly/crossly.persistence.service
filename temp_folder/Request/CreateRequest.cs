using Persistence.Repository;
using Persistence.Validation;

namespace Persistence.Request
{
    public class CreateRequest(CreateInput input, IRepository repository, IValidator validator) : RequestBase<CreateInput>(input)
    {
        protected override IResult? Validate(CreateInput input)
        {
            if (!validator.IsValidStream(input.DataModelStream))
            {
                return Results.BadRequest($"data model is invalid.");
            }

            return default;
        }

        protected override async Task<IResult> ExecuteRequest(CreateInput input)
        {
            IResult result = await repository.Create(input.DataModelStream);
            return result;
        }
    }

    public record CreateInput(Stream DataModelStream);
}