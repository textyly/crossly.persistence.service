using Persistence.Repository;
using Persistence.Validation;

namespace Persistence.Request
{
    public class RequestFactory(IRepository repository, IValidator validator) : IRequestFactory
    {
        public IRequest CreateGetAll(GetAllInput input)
        {
            GetAllRequest getAll = new(input, repository, validator);
            return getAll;
        }

        public IRequest CreateGetById(GetByIdInput input)
        {
            GetByIdRequest getById = new(input, repository, validator);
            return getById;
        }

        public IRequest CreateCreate(CreateInput input)
        {
            CreateRequest replace = new(input, repository, validator);
            return replace;
        }

        public IRequest CreateReplace(ReplaceInput input)
        {
            ReplaceRequest replace = new(input, repository, validator);
            return replace;
        }

        public IRequest CreateRename(RenameInput input)
        {
            RenameRequest rename = new(input, repository, validator);
            return rename;
        }

        public IRequest CreateDelete(DeleteInput input)
        {
            DeleteRequest delete = new(input, repository, validator);
            return delete;
        }
    }
}