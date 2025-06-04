using Persistence.Repository;
using Persistence.Validation;

namespace Persistence.Requests
{
    public class DeleteRequest : IRequest
    {
        private readonly IRepository repository;

        private readonly string? id;
        private readonly IResult? error;

        public DeleteRequest(DeleteInput input, IRepository repository, IValidator validator)
        {
            this.repository = repository;

            // TODO: export to validator
            if (validator.ValidateId(input.Id, out error))
            {
                id = input.Id;
            }
        }

        public async Task<IResult> Execute()
        {
            if (error is not null)
            {
                return error;
            }

            return await ExecuteCore(id!);
        }

        private async Task<IResult> ExecuteCore(string id)
        {
            IResult result = await repository.Delete(id);
            return result;
        }
    }

    public record DeleteInput(string Id);
}