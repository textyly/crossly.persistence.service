using Persistence.DataModel;
using Persistence.DataModel.Fabric;
using Persistence.DataModel.Pattern;
using Persistence.DataModel.Threads;
using Persistence.Persistence.BsonDataModel;

namespace Persistence.Conversion
{
    public class Converter : IConverter
    {
        public Version Version => new(0, 0, 0, 1); // increase on breaking change and keep in sync with data model's version!!!

        public BsonCrosslyDataModel Convert(CrosslyDataModel dataModel)
        {
            string version = Version.ToString();
            if (version != dataModel.Version)
            {
                throw new Exception($"version mismatch, converter version is {version} whereas data mode version is {dataModel.Version}");
            }

            BsonCrosslyDataModel bsonDataModel = new()
            {
                Version = dataModel.Version,
                Name = dataModel.Name,
                Fabric = CreateBsonFabricDataModel(dataModel.Fabric),
                Threads = CreateBsonThreadsDataModel(dataModel.Threads),
                Pattern = CreateBsonPatternDataModel(dataModel.Pattern)
            };

            return bsonDataModel;
        }

        public CrosslyDataModel Convert(BsonCrosslyDataModel bsonDataModel)
        {
            string version = Version.ToString();
            if (version != bsonDataModel.Version)
            {
                throw new ValidationException($"version mismatch, converter version is {version} whereas bson data mode version is {bsonDataModel.Version}");
            }

            string name = bsonDataModel.Name;
            FabricDataModel fabric = CreateFabricDataModel(bsonDataModel.Fabric);
            ThreadDataModel[] threads = CreateThreadsDataModel(bsonDataModel.Threads);
            ThreadPathDataModel[] pattern = CreatePatternDataModel(bsonDataModel.Pattern);

            CrosslyDataModel crosslyDataModel = new(bsonDataModel.Version, name, fabric, threads, pattern);

            return crosslyDataModel;
        }

        private BsonFabricDataModel CreateBsonFabricDataModel(FabricDataModel fabric)
        {
            BsonFabricDataModel bsonFabricDataModel = new()
            {
                Name = fabric.Name,
                Rows = fabric.Rows,
                Columns = fabric.Columns,
                Color = fabric.Color,
                Dots = new() { Color = fabric.Dots.Color },
                Threads = new() { Color = fabric.Threads.Color }
            };

            return bsonFabricDataModel;
        }

        private List<BsonThreadDataModel> CreateBsonThreadsDataModel(ThreadDataModel[] pattern)
        {
            List<BsonThreadDataModel> bsonThreadDataModels = [];

            foreach (ThreadDataModel threadDataModel in pattern)
            {
                BsonThreadDataModel bsonThreadDataModel = CreateBsonThreadDataModel(threadDataModel);
                bsonThreadDataModels.Add(bsonThreadDataModel);
            }

            return bsonThreadDataModels;
        }

        private BsonThreadDataModel CreateBsonThreadDataModel(ThreadDataModel thread)
        {
            BsonThreadDataModel bsonThreadDataModel = new()
            {
                Name = thread.Name,
                Color = thread.Color,
                Width = thread.Width
            };

            return bsonThreadDataModel;
        }

        private List<BsonThreadPathDataModel> CreateBsonPatternDataModel(ThreadPathDataModel[] threadPaths)
        {
            List<BsonThreadPathDataModel> bsonPatternDataModels = [];

            foreach (ThreadPathDataModel threadPathDataModel in threadPaths)
            {
                BsonThreadPathDataModel bsonThreadPathDataModel = CreateBsonPatternDataModel(threadPathDataModel);
                bsonPatternDataModels.Add(bsonThreadPathDataModel);
            }

            return bsonPatternDataModels;
        }

        private BsonThreadPathDataModel CreateBsonPatternDataModel(ThreadPathDataModel threadPath)
        {
            BsonThreadPathDataModel bsonPatternDataModel = new()
            {
                ThreadIndex = threadPath.ThreadIndex,
                NeedlePath = new()
                {
                    IndexesX = [.. threadPath.NeedlePath.IndexesX],
                    IndexesY = [.. threadPath.NeedlePath.IndexesY]
                }
            };

            return bsonPatternDataModel;
        }

        private FabricDataModel CreateFabricDataModel(BsonFabricDataModel bsonFabric)
        {
            FabricDataModel fabricDataModel = new(
                bsonFabric.Name,
                bsonFabric.Columns,
                bsonFabric.Rows,
                bsonFabric.Color,
                new(bsonFabric.Dots.Color),
                new(bsonFabric.Threads.Color));

            return fabricDataModel;
        }

        private ThreadDataModel[] CreateThreadsDataModel(List<BsonThreadDataModel> bsonTreads)
        {
            var threadDataModels = new ThreadDataModel[bsonTreads.Count];

            for (int i = 0; i < bsonTreads.Count; i++)
            {
                BsonThreadDataModel bsonThreadDataModel = bsonTreads[i];
                threadDataModels[i] = CreateThreadDataModel(bsonThreadDataModel);
            }

            return threadDataModels;
        }

        private ThreadDataModel CreateThreadDataModel(BsonThreadDataModel bsonTread)
        {
            ThreadDataModel threadDataModel = new(
                bsonTread.Name,
                bsonTread.Color,
                bsonTread.Width
            );

            return threadDataModel;
        }

        private ThreadPathDataModel[] CreatePatternDataModel(List<BsonThreadPathDataModel> bsonPattern)
        {
            var patternDataModels = new ThreadPathDataModel[bsonPattern.Count];

            for (int i = 0; i < bsonPattern.Count; i++)
            {
                BsonThreadPathDataModel bsonPatternDataModel = bsonPattern[i];
                patternDataModels[i] = CreateThreadPathDataModel(bsonPatternDataModel);
            }

            return patternDataModels;
        }

        private ThreadPathDataModel CreateThreadPathDataModel(BsonThreadPathDataModel bsonThreadPath)
        {
            ThreadPathDataModel threadPathDataModel = new(
                bsonThreadPath.ThreadIndex,
                new([.. bsonThreadPath.NeedlePath.IndexesX], [.. bsonThreadPath.NeedlePath.IndexesY])
            );

            return threadPathDataModel;
        }
    }
}