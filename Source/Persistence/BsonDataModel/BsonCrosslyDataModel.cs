using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Persistence.Persistence.BsonDataModel
{
    public class BsonCrosslyDataModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("fabric")]
        public BsonFabricDataModel Fabric { get; set; } = null!;

        [BsonElement("threads")]
        public List<BsonThreadDataModel> Threads { get; set; } = [];

        [BsonElement("pattern")]
        public List<BsonPatternDataModel> Pattern { get; set; } = [];
    }

    public class BsonFabricDataModel
    {
        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("columns")]
        public int Columns { get; set; }

        [BsonElement("rows")]
        public int Rows { get; set; }

        [BsonElement("color")]
        public string Color { get; set; } = null!;

        [BsonElement("dots")]
        public BsonDotsDataModel Dots { get; set; } = null!;

        [BsonElement("threads")]
        public BsonThreadsDataModel Threads { get; set; } = null!;
    }

    public class BsonDotsDataModel
    {
        [BsonElement("color")]
        public string Color { get; set; } = null!;
    }

    public class BsonThreadsDataModel
    {
        [BsonElement("color")]
        public string Color { get; set; } = null!;
    }

    public class BsonThreadDataModel
    {
        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("color")]
        public string Color { get; set; } = null!;

        [BsonElement("width")]
        public int Width { get; set; }
    }

    public class BsonPatternDataModel
    {
        [BsonElement("threadIndex")]
        public int ThreadIndex { get; set; }

        [BsonElement("needlePath")]
        public BsonNeedlePathDataModel NeedlePath { get; set; } = null!;
    }

    public class BsonNeedlePathDataModel
    {
        [BsonElement("indexesX")]
        public List<int> IndexesX { get; set; } = [];

        [BsonElement("indexesY")]
        public List<int> IndexesY { get; set; } = [];
    }
}