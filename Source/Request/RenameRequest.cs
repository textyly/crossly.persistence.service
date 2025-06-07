using Persistence.Repository;
using Persistence.Validation;

namespace Persistence.Request
{
    public class RenameRequest(RenameInput input, IRepository repository, IValidator validator) : RequestBase<RenameInput>(input)
    {
        protected override IResult? Validate(RenameInput input)
        {
            if (!validator.IsValidId(input.Id))
            {
                return Results.BadRequest($"{nameof(input.Id)} is not valid.");
            }

            if (!validator.IsValidName(input.NewName))
            {
                return Results.BadRequest($"{nameof(input.NewName)} is not valid.");
            }

            return default;
        }

        protected override async Task<IResult> ExecuteRequest(RenameInput input)
        {
            try
            {
                IResult result = await repository.Rename(input.Id, input.NewName);
                return result;
            }
            catch
            {
                return Results.InternalServerError();
            }
        }
    }

    public record RenameInput(string Id, string NewName);
}