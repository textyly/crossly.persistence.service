using Persistence.DataModel;

namespace Persistence.Persistence
{
    public interface IPersistence
    {

        Task Start();

        /// <summary>
        /// TBD 
        /// </summary>
        /// <param name="dataModel">TBD</param>
        /// <returns>id of the persisted data model</returns>
        Task<string> Save(CrosslyDataModel dataModel);

        /// <summary>
        /// TBD
        /// </summary>
        /// <param name="id">TBD</param>
        /// <returns>TBD</returns>
        Task<CrosslyDataModel?> Get(string id);
    }
}