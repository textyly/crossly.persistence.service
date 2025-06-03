using Persistence.DataModel;

namespace Persistence.Validation
{
    public class Validator : IValidator
    {
        public void ThrowIfInvalid(string id)
        {
            // TODO: 
        }

        public void ThrowIfInvalid(string id, string newName)
        {
            // TODO: 
        }

        public void ThrowIfInvalid(Stream? dataModelStream)
        {
            // TODO: 
        }

        public void ThrowIfInvalid(CrosslyDataModel? dataModel)
        {
            // TODO: 
        }
    }
}