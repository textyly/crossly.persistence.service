using MongoDB.Driver;
using Persistence.DataModel;
using Persistence.Conversion;
using Persistence.Persistence.BsonDataModel;

namespace Persistence.Persistence
{
    public class MongoDbPersistence : IPersistence
    {
        private readonly IConverter converter;
        private readonly string dbName = "CrosslyDb";
        private readonly string collectionName = "DataModels";
        private readonly string connectionString = "mongodb://localhost:27017";
        private readonly IMongoCollection<BsonCrosslyDataModel> dataModelsCollection;

        public MongoDbPersistence(IConverter converter)
        {
            this.converter = converter;

            MongoClient client = new(connectionString);
            IMongoDatabase database = client.GetDatabase(dbName);
            dataModelsCollection = database.GetCollection<BsonCrosslyDataModel>(collectionName);
        }

        public async Task Start()
        {
            // TODO: do we need index???
            IndexKeysDefinition<BsonCrosslyDataModel> indexKeys =
                Builders<BsonCrosslyDataModel>.IndexKeys.Ascending(p => p.Name);

            CreateIndexOptions indexOptions = new() { Unique = true };
            CreateIndexModel<BsonCrosslyDataModel> indexModel = new(indexKeys, indexOptions);

            await dataModelsCollection.Indexes.CreateOneAsync(indexModel);
        }

        public async Task<string[]> GetAll()
        {
            var query = dataModelsCollection.Find(_ => true).Project(x => x.Id);
            IEnumerable<string?> queryResult = await query.ToListAsync();

            string[] ids = queryResult.Where(id => id is not null).Select(id => id!).ToArray();
            return ids;
        }

        public async Task<CrosslyDataModel?> GetById(string id)
        {
            var query = dataModelsCollection.Find(p => p.Id == id);
            BsonCrosslyDataModel bsonDataModel = await query.FirstOrDefaultAsync();

            return bsonDataModel is null
                ? default
                : converter.Convert(bsonDataModel);
        }

        public async Task<CrosslyDataModel?> GetByName(string name)
        {
            var query = dataModelsCollection.Find(p => p.Name == name);
            BsonCrosslyDataModel bsonDataModel = await query.FirstOrDefaultAsync();

            return bsonDataModel is null
                ? default
                : converter.Convert(bsonDataModel);
        }

        public async Task<string> Save(CrosslyDataModel dataModel)
        {
            BsonCrosslyDataModel bsonDataModel = converter.Convert(dataModel);
            await dataModelsCollection.InsertOneAsync(bsonDataModel);

            return bsonDataModel.Id is null
                ? throw new Exception("data model id cannot be null")
                : bsonDataModel.Id;
        }

        public async Task<bool> Replace(string id, CrosslyDataModel newDataModel)
        {
            BsonCrosslyDataModel replacementDataModel = converter.Convert(newDataModel);
            replacementDataModel.Id = id;

            ReplaceOneResult result = await dataModelsCollection.ReplaceOneAsync(p => p.Id == id && p.Name == newDataModel.Name, replacementDataModel);

            bool success = result.IsAcknowledged && result.ModifiedCount == 1;
            return success;
        }

        public async Task<bool> Rename(string id, string newName)
        {
            FilterDefinition<BsonCrosslyDataModel> filter = Builders<BsonCrosslyDataModel>.Filter.And(
                    Builders<BsonCrosslyDataModel>.Filter.Eq(p => p.Id, id));

            UpdateDefinition<BsonCrosslyDataModel> update = Builders<BsonCrosslyDataModel>.Update.Set(p => p.Name, newName);

            UpdateResult result = await dataModelsCollection.UpdateOneAsync(filter, update);

            bool success = result.IsAcknowledged && result.ModifiedCount == 1;
            return success;
        }
    }
}