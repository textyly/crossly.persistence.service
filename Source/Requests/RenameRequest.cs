using Persistence.Repository;
using Persistence.Validation;

namespace Persistence.Requests
{
    public class RenameRequest : IRequest
    {
        private readonly IRepository repository;
        private readonly IValidator validator;

        private readonly string? id;
        private readonly string? newName;
        private readonly IResult? error;

        public RenameRequest(RenameInput input, IRepository repository, IValidator validator)
        {
            this.repository = repository;
            this.validator = validator;

            if (!validator.ValidateRename(input.Id, input.NewName, out error))
            {
                id = input.Id;
                newName = input.NewName;
            }
        }

        public async Task<IResult> Execute() => error ?? await Execute(id!, newName!);

        private async Task<IResult> Execute(string id, string newName)
        {
            IResult result = await repository.Rename(id, newName);
            return result;
        }
    }

    public record RenameInput(string Id, string NewName);
}