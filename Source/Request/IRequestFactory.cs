namespace Persistence.Request
{
    public interface IRequestFactory
    {
        IRequest CreateGetAll(GetAllInput input);
        IRequest CreateGetById(GetByIdInput input);
        IRequest CreateCreate(CreateInput input);
        IRequest CreateReplace(ReplaceInput input);
        IRequest CreateRename(RenameInput input);
        IRequest CreateDelete(DeleteInput input);
    }
}