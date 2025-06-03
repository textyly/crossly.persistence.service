using Persistence.DataModel;

namespace Persistence.Validation
{
    public class Validator : IValidator
    {
        public void ThrowIfInvalid(string id)
        {
            throw new NotImplementedException();
        }

        public void ThrowIfInvalid(string id, string newName)
        {
            throw new NotImplementedException();
        }

        public void ThrowIfInvalid(Stream dataModelStream)
        {
            throw new NotImplementedException();
        }

        public void ThrowIfInvalid(CrosslyDataModel? dataModel)
        {
            throw new NotImplementedException();
        }
    }
}