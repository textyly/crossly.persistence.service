using Persistence.DataModel;

namespace Persistence.Validation
{
    public class Validator : IValidator
    {
        public bool IsValidId(string? id) => !string.IsNullOrWhiteSpace(id);
        public bool IsValidName(string? name) => !string.IsNullOrWhiteSpace(name);

        public bool IsValidStream(Stream? dataModelStream)
        {
            return dataModelStream is not null && dataModelStream.Length > 0;
        }

        public bool IsValidDataModel(CrosslyDataModel? dataModel)
        {
            return dataModel is not null && ValidateCrosslyDataModel(dataModel);
        }

        public bool IsValidRename(string? id, string? name)
        {
            return !IsValidId(id) && !IsValidName(name);
        }

        private bool ValidateCrosslyDataModel(CrosslyDataModel dataModel)
        {
            // TODO: test!!!

            if (string.IsNullOrWhiteSpace(dataModel.Name))
            {
                return false;
            }

            return true;
        }
    }
}