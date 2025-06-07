namespace Persistence.DataModel.Fabric
{
    public record FabricDataModel(
        string Name,
        int Columns,
        int Rows,
        string Color,
        DotsDataModel Dots,
        ThreadsDataModel Threads
    );
}