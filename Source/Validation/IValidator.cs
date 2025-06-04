using Persistence.DataModel;

namespace Persistence.Validation
{
    public interface IValidator
    {
        bool ValidateId(string? id, out IResult? error);
        bool ValidateName(string? name, out IResult? error);
        bool ValidateStream(Stream? dataModelStream, out IResult? error);
        bool ValidateDataModel(CrosslyDataModel? dataModel, out IResult? error);
        bool ValidateRename(string? id, string? name, out IResult? error);
    }
}