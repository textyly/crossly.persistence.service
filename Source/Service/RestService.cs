using Persistence.DataModel;
using Persistence.Validation;
using Persistence.Compression;
using Persistence.Persistence;

namespace Persistence.Service
{
    public class RestService
    {
        private readonly string rootPath;
        private readonly string idPath;
        private readonly string createdUri;
        private readonly string renamePath;
        private readonly WebApplication webApp;
        private readonly IValidator validator;
        private readonly ICompressor compressor;
        private readonly IPersistence persistence;

        public RestService(WebApplication webApp, IValidator validator, ICompressor compressor, IPersistence persistence)
        {
            this.webApp = webApp;
            this.validator = validator;
            this.compressor = compressor;
            this.persistence = persistence;

            rootPath = "/api/v1/patterns";      // -> /api/v1/patterns
            idPath = $"{rootPath}/{{id}}";      // -> /api/v1/patterns/{id}
            renamePath = $"{idPath}/rename";    // -> /api/v1/patterns/{id}/rename

            createdUri = $"{rootPath}/{{0}}";   // -> /api/v1/patterns/{0}
        }

        public void RegisterMethods()
        {
            RegisterGetAll();    // -> /api/v1/patterns
            RegisterGetById();   // -> /api/v1/patterns/abc123
            RegisterCreate();    // -> /api/v1/patterns
            RegisterReplace();   // -> /api/v1/patterns/abc123
            RegisterRename();    // -> /api/v1/patterns/abc123/rename
            RegisterDelete();    // -> /api/v1/patterns/abc123
        }

        private void RegisterGetAll() => webApp.MapGet(rootPath, GetAllHandler);
        private void RegisterGetById() => webApp.MapGet(idPath, GetByIdHandler);
        private void RegisterCreate() => webApp.MapPost(rootPath, (Delegate)CreateHandler);
        private void RegisterReplace() => webApp.MapPut(idPath, ReplaceHandler);
        private void RegisterRename() => webApp.MapPatch(renamePath, RenameHandler);
        private void RegisterDelete() => webApp.MapDelete(idPath, DeleteHandler);

        private Task<IResult> GetAllHandler()
        {
            // no validation

            return GetAll();
        }

        private async Task<IResult> GetByIdHandler(string id)
        {
            // validation first:
            validator.ThrowIfInvalid(id);

            CrosslyDataModel? dataModel = await persistence.GetById(id);
            validator.ThrowIfInvalid(dataModel);

            Stream? dataModelStream = await compressor.CompressToStream(dataModel!);
            validator.ThrowIfInvalid(dataModelStream);

            // then real work:
            return Results.File(dataModelStream, "application/octet-stream");
        }

        public async Task<IResult> CreateHandler(HttpContext httpContext)
        {
            // validation first:
            Stream dataModelStream = httpContext.Request.Body;
            validator.ThrowIfInvalid(dataModelStream);

            CrosslyDataModel? dataModel = await compressor.TryDecompressToDataModel(dataModelStream);
            validator.ThrowIfInvalid(dataModel);

            // then real work:
            return await Create(dataModel!);
        }

        private async Task<IResult> ReplaceHandler(string id, HttpContext httpContext)
        {
            // validation first:
            validator.ThrowIfInvalid(id);

            Stream dataModelStream = httpContext.Request.Body;
            validator.ThrowIfInvalid(dataModelStream);

            CrosslyDataModel? dataModel = await compressor.TryDecompressToDataModel(dataModelStream);
            validator.ThrowIfInvalid(dataModel);

            // then real work:
            return await Replace(id, dataModel!);
        }

        private async Task<IResult> RenameHandler(string id, RenamePatternRequest request)
        {
            // validation first:
            string newName = request.NewName;
            validator.ThrowIfInvalid(id, newName);

            // then real work:
            return await Rename(id, newName);
        }

        private async Task<IResult> DeleteHandler(string id)
        {
            // validation first:
            validator.ThrowIfInvalid(id);

            // then real work:
            return await Delete(id);
        }

        private async Task<IResult> GetAll()
        {
            string[] ids = await persistence.GetAll();
            return Results.Ok(new { ids });
        }

        private async Task<IResult> Create(CrosslyDataModel dataModel)
        {
            string id = await persistence.Create(dataModel!);
            string uri = string.Format(createdUri, id);
            return Results.Created(uri, new { id });
        }

        private async Task<IResult> Replace(string id, CrosslyDataModel dataModel)
        {
            bool success = await persistence.Replace(id, dataModel!);
            return success ? Results.Ok() : Results.NotFound();
        }

        private async Task<IResult> Rename(string id, string newName)
        {
            bool success = await persistence.Rename(id, newName);
            return success ? Results.Ok() : Results.NotFound();
        }

        private async Task<IResult> Delete(string id)
        {
            bool success = await persistence.Delete(id);
            return success ? Results.NoContent() : Results.NotFound();
        }
    }

    public record RenamePatternRequest(string NewName);
}