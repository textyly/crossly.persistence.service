using Persistence.DataModel;

namespace Persistence.Conversion
{
    public interface IConverter
    {
        Task<CrosslyDataModel?> TryConvertToDataModel(Stream dataModelStream);
        Task<Stream> ConvertToStream(CrosslyDataModel dataModel);
    }
}