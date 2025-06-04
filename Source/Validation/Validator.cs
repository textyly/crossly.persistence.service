using Persistence.DataModel;

namespace Persistence.Validation
{
    public class Validator : IValidator
    {
        public bool ValidateId(string? id, out IResult? error)
        {
            bool valid = !string.IsNullOrWhiteSpace(id);
            // TODO: validate mongodb id
            error = !valid ? default : Results.BadRequest($"id is not valid.");
            return valid;
        }

        public bool ValidateName(string? name, out IResult? error)
        {
            bool valid = !string.IsNullOrWhiteSpace(name);

            error = !valid ? default : Results.BadRequest($"name is not valid.");
            return valid;
        }

        public bool ValidateStream(Stream? dataModelStream, out IResult? error)
        {
            bool valid = dataModelStream is not null && dataModelStream.Length > 0;

            error = !valid ? default : Results.BadRequest($"http body is not valid.");
            return valid;
        }

        public bool ValidateDataModel(CrosslyDataModel? dataModel, out IResult? error)
        {
            bool valid = dataModel is not null && ValidateCrosslyDataModel(dataModel);

            error = !valid ? default : Results.BadRequest($"data model is not valid.");
            return valid;
        }

        public bool ValidateRename(string? id, string? name, out IResult? error)
        {
            if (!ValidateId(id, out error))
            {
                return false;
            }

            if (!ValidateName(name, out error))
            {
                return false;
            }

            return true;
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