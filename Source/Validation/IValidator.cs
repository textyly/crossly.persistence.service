using Persistence.DataModel;

namespace Persistence.Validation
{
    public interface IValidator
    {
        Version Version { get; }

        bool IsValidId(string? id);
        bool IsValidName(string? name);
        bool IsValidStream(Stream? dataModelStream);
        bool IsValidDataModel(CrosslyDataModel? dataModel);
    }
}