using Persistence.Requests;

namespace Persistence.Handler
{
    public class RequestHandler(IRequestFactory requestFactory) : IRequestHandler
    {
        public async Task<IResult> GetAll()
        {
            IRequest getAll = requestFactory.CreateGetAll();
            IResult result = await getAll.Execute();

            return result;
        }

        public async Task<IResult> GetById(string id)
        {
            GetByIdInput input = new(id);

            IRequest getById = requestFactory.CreateGetById(input);
            IResult result = await getById.Execute();

            return result;
        }

        public async Task<IResult> Create(HttpContext httpContext)
        {
            Stream dataModelStream = httpContext.Request.Body;
            CreateInput input = new(dataModelStream);

            IRequest create = requestFactory.CreateCreate(input);
            IResult result = await create.Execute();

            return result;
        }

        public async Task<IResult> Replace(string id, HttpContext httpContext)
        {
            Stream dataModelStream = httpContext.Request.Body;
            ReplaceInput input = new(id, dataModelStream);

            IRequest replace = requestFactory.CreateReplace(input);
            IResult result = await replace.Execute();

            return result;
        }

        public async Task<IResult> Rename(string id, RenameRequestInput request)
        {
            RenameInput input = new(id, request.NewName);

            IRequest rename = requestFactory.CreateRename(input);
            IResult result = await rename.Execute();

            return result;
        }

        public async Task<IResult> Delete(string id)
        {
            DeleteInput input = new(id);

            IRequest delete = requestFactory.CreateDelete(input);
            IResult result = await delete.Execute();

            return result;
        }
    }

    public record RenameRequestInput(string NewName);
}