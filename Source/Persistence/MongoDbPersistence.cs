using MongoDB.Driver;
using Persistence.Conversion;
using Persistence.DataModel;
using Persistence.Persistence.BsonDataModel;

namespace Persistence.Persistence
{
    public class MongoDbPersistence(IConverter converter) : IPersistence
    {
        private readonly IConverter converter = converter;
        private readonly string dbName = "CrosslyDb";
        private readonly string collectionName = "DataModels";
        private readonly string connectionString = "mongodb://localhost:27017";

        private IMongoCollection<BsonCrosslyDataModel>? dataModelsCollection;

        public async Task Start()
        {
            MongoClient client = new(connectionString);
            IMongoDatabase database = client.GetDatabase(dbName);
            dataModelsCollection = database.GetCollection<BsonCrosslyDataModel>(collectionName);

            IndexKeysDefinition<BsonCrosslyDataModel> indexKeys =
                Builders<BsonCrosslyDataModel>.IndexKeys.Ascending(p => p.Name);

            CreateIndexOptions indexOptions = new() { Unique = true };
            CreateIndexModel<BsonCrosslyDataModel> indexModel = new(indexKeys, indexOptions);

            await dataModelsCollection.Indexes.CreateOneAsync(indexModel);
        }

        public async Task<CrosslyDataModel?> Get(string id)
        {
            BsonCrosslyDataModel? bsonDataModel = await dataModelsCollection
                                                            .Find(p => p.Id == id)
                                                            .FirstOrDefaultAsync();

            CrosslyDataModel? dataModel = converter.Convert(bsonDataModel);
            return dataModel;
        }

        public async Task<string> Save(CrosslyDataModel dataModel)
        {
            BsonCrosslyDataModel bsonDataModel = converter.Convert(dataModel);
            await dataModelsCollection!.InsertOneAsync(bsonDataModel);

            string id = bsonDataModel.Id!;
            return id;
        }
    }
}