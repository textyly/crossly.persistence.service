using MongoDB.Bson;
using MongoDB.Driver;
using Persistence.DataModel;
using Persistence.Persistence.BsonDataModel;

namespace Persistence.Persistence
{
    public class MongoDbPersistence : IPersistence
    {
        private readonly string dbName;
        private readonly string collectionName;
        private readonly string connectionString;

        private IMongoCollection<BsonCrosslyDataModel>? patternsCollection;

        public MongoDbPersistence()
        {
            dbName = "CrosslyDb";
            collectionName = "Patterns";
            connectionString = "mongodb://localhost:27017";
        }

        public async Task Start()
        {
            MongoClient client = new(connectionString);
            IMongoDatabase database = client.GetDatabase(dbName);
            patternsCollection = database.GetCollection<BsonCrosslyDataModel>(collectionName);

            IndexKeysDefinition<BsonCrosslyDataModel> indexKeys =
                Builders<BsonCrosslyDataModel>.IndexKeys.Ascending(p => p.Name);

            CreateIndexOptions indexOptions = new() { Unique = true };
            CreateIndexModel<BsonCrosslyDataModel> indexModel = new(indexKeys, indexOptions);

            await patternsCollection.Indexes.CreateOneAsync(indexModel);
        }

        public async Task<CrosslyDataModel?> Get(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                throw new ArgumentException("Invalid pattern ID format.");
            }
            else
            {
                BsonCrosslyDataModel? pattern = await patternsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
                CrosslyDataModel? dataModel = null; // TODO: implement
                return dataModel;
            }
        }

        public async Task<string> Save(CrosslyDataModel dataModel)
        {
            BsonCrosslyDataModel bsonDataModel = null!; // TODO: implement
            await patternsCollection!.InsertOneAsync(bsonDataModel);

            string id = bsonDataModel.Id!;
            return id;
        }
    }
}