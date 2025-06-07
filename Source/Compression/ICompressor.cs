using Persistence.DataModel;

namespace Persistence.Compression
{
    public interface ICompressor
    {
        Task<CrosslyDataModel?> DecompressToDataModel(Stream dataModelStream);
        Task<Stream> CompressToStream(CrosslyDataModel dataModel);
    }
}