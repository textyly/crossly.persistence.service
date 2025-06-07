using Persistence.DataModel;
using Persistence.DataModel.Fabric;
using Persistence.DataModel.Pattern;
using Persistence.DataModel.Threads;

namespace Persistence.Validation
{
    public class Validator : IValidator
    {
        public Version Version => new(0, 0, 0, 1); // increase on breaking change and keep in sync with data model's version!!!
        public bool IsValidId(string? id) => !string.IsNullOrWhiteSpace(id);
        public bool IsValidName(string? name) => !string.IsNullOrWhiteSpace(name);

        public bool IsValidStream(Stream? dataModelStream)
        {
            return dataModelStream is not null;
        }

        public bool IsValidDataModel(CrosslyDataModel? dataModel)
        {
            return dataModel is not null && IsValidCrosslyDataModel(dataModel);
        }

        public bool IsValidRename(string? id, string? name)
        {
            return !IsValidId(id) && !IsValidName(name);
        }

        private bool IsValidCrosslyDataModel(CrosslyDataModel dataModel)
        {
            if (string.IsNullOrWhiteSpace(dataModel?.Name))
            {
                return false;
            }

            return
                IsValidFabricDataModel(dataModel.Fabric) &&
                IsValidThreadsDataModel(dataModel.Threads) &&
                IsValidPatternDataModel(dataModel.Pattern);
        }

        private bool IsValidFabricDataModel(FabricDataModel fabric)
        {
            return
                fabric?.Name?.Length > 0 &&
                fabric?.Columns > 0 &&
                fabric?.Rows > 0 &&
                fabric?.Color?.Length > 0 &&
                fabric?.Dots?.Color?.Length > 0 &&
                fabric?.Threads?.Color?.Length > 0;
        }

        private bool IsValidThreadsDataModel(ThreadDataModel[] threads)
        {
            if (threads is null)
            {
                return false;
            }

            foreach (ThreadDataModel thread in threads)
            {
                if (thread?.Name?.Length <= 0 ||
                    thread?.Color?.Length <= 0 ||
                    thread?.Width <= 0)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsValidPatternDataModel(ThreadPathDataModel[] pattern)
        {
            if (pattern is null)
            {
                return false;
            }

            foreach (ThreadPathDataModel path in pattern)
            {
                if (path?.ThreadIndex < 0 ||
                    path?.NeedlePath?.IndexesX is null ||
                    path?.NeedlePath?.IndexesY is null ||
                    path.NeedlePath.IndexesX.Length != path.NeedlePath.IndexesY.Length)
                {
                    return false;
                }

                int[] indexesX = path.NeedlePath.IndexesX;
                int[] indexesY = path.NeedlePath.IndexesY;

                for (int i = 0; i < indexesX.Length; i++)
                {
                    int x = indexesX[i];
                    int y = indexesY[i];

                    if (x < 0 || y < 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}