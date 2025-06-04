using Persistence.Repository;
using Persistence.Validation;
using Persistence.Compression;

namespace Persistence.Requests
{
    public class RequestFactory(IValidator validator, ICompressor compressor, IRepository repository) : IRequestFactory
    {
        public IRequest CreateGetAll()
        {
            GetAllRequest getAll = new(repository);
            return getAll;
        }

        public IRequest CreateGetById(GetByIdInput input)
        {
            GetByIdRequest getById = new(input, repository, validator, compressor);
            return getById;
        }

        public IRequest CreateCreate(CreateInput input)
        {
            CreateRequest replace = new(input, repository, validator, compressor);
            return replace;
        }

        public IRequest CreateReplace(ReplaceInput input)
        {
            ReplaceRequest replace = new(input, repository, validator, compressor);
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