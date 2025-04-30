using Persistence.DataModel.Fabric;
using Persistence.DataModel.Pattern;
using Persistence.DataModel.Threads;

namespace Persistence.DataModel
{
    public record CrosslyDataModel(
        string Name,
        FabricDataModel Fabric,
        ThreadDataModel[] Threads,
        ThreadPathDataModel[] Pattern
    );
}