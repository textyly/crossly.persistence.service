using Persistence.DataModel;
using Persistence.Persistence.BsonDataModel;

namespace Persistence.Conversion
{
    public interface IConverter
    {
        BsonCrosslyDataModel Convert(CrosslyDataModel dataModel);
        CrosslyDataModel Convert(BsonCrosslyDataModel dataModel);
    }
}