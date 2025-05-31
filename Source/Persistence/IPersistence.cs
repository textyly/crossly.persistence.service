using Persistence.DataModel;

namespace Persistence.Persistence
{
    public interface IPersistence
    {

        Task Start();

        /// <summary>
        /// TBD
        /// </summary>
        /// <returns></returns>
        Task<string[]> GetAll();

        /// <summary>
        /// TBD
        /// </summary>
        /// <param name="id">TBD</param>
        /// <returns>TBD</returns>
        Task<CrosslyDataModel?> GetById(string id);

        /// <summary>
        /// TBD
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<CrosslyDataModel?> GetByName(string name);

        /// <summary>
        /// TBD 
        /// </summary>
        /// <param name="dataModel">TBD</param>
        /// <returns>id of the persisted data model</returns>
        Task<string> Save(CrosslyDataModel dataModel);

        /// <summary>
        /// TBD
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newDataModel"></param>
        /// <returns></returns>
        Task<bool> Replace(string id, CrosslyDataModel newDataModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        Task<bool> Rename(string id, string newName);
    }
}