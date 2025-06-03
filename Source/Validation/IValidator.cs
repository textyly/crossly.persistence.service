using Persistence.DataModel;

namespace Persistence.Validation
{
    public interface IValidator
    {
        void ThrowIfInvalid(string id);
        void ThrowIfInvalid(string id, string newName);
        void ThrowIfInvalid(Stream? dataModelStream);
        void ThrowIfInvalid(CrosslyDataModel? dataModel);
    }
}