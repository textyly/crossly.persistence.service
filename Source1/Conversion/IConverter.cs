using Persistence.DataModel;
using Persistence.Persistence.BsonDataModel;

namespace Persistence.Conversion
{
    public interface IConverter
    {
        public Version Version { get; }
        BsonCrosslyDataModel Convert(CrosslyDataModel dataModel);
        CrosslyDataModel Convert(BsonCrosslyDataModel dataModel);
    }
}