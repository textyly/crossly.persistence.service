using Persistence.DataModel;
using System.Collections.Concurrent;

namespace Persistence.Persistence
{
    public class MongoDbPersistence : IPersistence
    {
        private volatile int nextId = -1;
        private readonly ConcurrentDictionary<string, CrosslyDataModel> database;

        public MongoDbPersistence()
        {
            database = new();
        }

        public Task<CrosslyDataModel?> Get(string id)
        {
            database.TryGetValue(id, out CrosslyDataModel? dataModel);
            return Task.FromResult(dataModel);
        }

        public Task<string> Save(CrosslyDataModel dataModel)
        {
            int id = Interlocked.Increment(ref nextId);

            string strId = id.ToString();
            database.AddOrUpdate(strId, key => dataModel, (key, oldValue) => dataModel); // just replace if exists

            return Task.FromResult(strId);
        }
    }
}