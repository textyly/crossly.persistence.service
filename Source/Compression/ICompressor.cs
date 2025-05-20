using Persistence.DataModel;

namespace Persistence.Compression
{
    public interface ICompressor
    {
        Task<CrosslyDataModel?> TryDecompressToDataModel(Stream dataModelStream);
        Task<Stream> CompressToStream(CrosslyDataModel dataModel);
    }
}