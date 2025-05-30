using MongoDB.Driver;
using Persistence.DataModel;
using Persistence.Conversion;
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

        public async Task<string[]> GetAll()
        {
            IEnumerable<BsonCrosslyDataModel> bsonDataModels = (await dataModelsCollection.FindAsync(Builders<BsonCrosslyDataModel>.Filter.Empty)).ToEnumerable();

            string[] ids = [.. bsonDataModels
                .Where(dataModel => dataModel.Id is not null)
                .Select(dataModel => dataModel.Id!)];

            return ids;
        }

        public async Task<CrosslyDataModel?> GetById(string id)
        {
            BsonCrosslyDataModel? bsonDataModel = await dataModelsCollection
                                                            .Find(p => p.Id == id)
                                                            .FirstOrDefaultAsync();

            CrosslyDataModel? dataModel = converter.Convert(bsonDataModel);
            return dataModel;
        }

        public async Task<CrosslyDataModel?> GetByName(string name)
        {
            BsonCrosslyDataModel? bsonDataModel = await dataModelsCollection
                                                            .Find(p => p.Name == name)
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