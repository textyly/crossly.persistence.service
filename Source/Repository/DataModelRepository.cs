using Persistence.DataModel;
using Persistence.Persistence;

namespace Persistence.Repository
{
    // TODO: add OpenAPI support
    public class DataModelRepository(string rootPath, IPersistence persistence) : IRepository
    {
        private readonly string createdUri = $"{rootPath}/{{0}}";   // -> /api/v1/patterns/{0}

        public async Task<IResult> GetAll()
        {
            try
            {
                string[] ids = await persistence.GetAll();
                return Results.Ok(new { ids });
            }
            catch (Exception e)
            {
                // TODO: log and send metric
                return Results.InternalServerError();
            }
        }

        public async Task<CrosslyDataModel?> GetById(string id)
        {
            try
            {
                CrosslyDataModel? dataModel = await persistence.GetById(id);
                return dataModel;
            }
            catch
            {
                // TODO: log and send metric
                throw;
            }
        }

        public async Task<IResult> Create(CrosslyDataModel dataModel)
        {
            try
            {
                string id = await persistence.Create(dataModel!);
                string uri = string.Format(createdUri, id);
                return Results.Created(uri, new { id });
            }
            catch (Exception e)
            {
                // TODO: log and send metric
                return Results.InternalServerError();
            }
        }

        public async Task<IResult> Replace(string id, CrosslyDataModel dataModel)
        {
            try
            {
                bool success = await persistence.Replace(id, dataModel!);
                return success ? Results.Ok() : Results.NotFound();
            }
            catch (Exception e)
            {
                // TODO: log and send metric
                return Results.InternalServerError();
            }
        }

        public async Task<IResult> Rename(string id, string newName)
        {
            try
            {
                bool success = await persistence.Rename(id, newName);
                return success ? Results.Ok() : Results.NotFound();
            }
            catch (Exception e)
            {
                // TODO: log and send metric
                return Results.InternalServerError();
            }
        }

        public async Task<IResult> Delete(string id)
        {
            try
            {
                bool success = await persistence.Delete(id);
                return success ? Results.NoContent() : Results.NotFound();
            }
            catch (Exception e)
            {
                // TODO: log and send metric
                return Results.InternalServerError();
            }
        }
    }
}