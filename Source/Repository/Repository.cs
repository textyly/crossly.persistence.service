using Persistence.DataModel;
using Persistence.Validation;
using Persistence.Compression;
using Persistence.Persistence;

namespace Persistence.Repository
{
    public class Repository(IPersistence persistence, IValidator validator, ICompressor compressor, LinkGenerator linkGenerator) : IRepository
    {
        public async Task<IResult> GetAll()
        {
            string[] ids = await persistence.GetAll();

            return Results.Ok(new { ids });
        }

        public async Task<IResult> GetById(string id)
        {
            CrosslyDataModel? dataModel = await persistence.GetById(id);

            return dataModel is null
                ? Results.NotFound($"{nameof(dataModel)} not found.")
                : await CreateGetByIdResult(dataModel);
        }

        public async Task<IResult> Create(Stream dataModelStream)
        {
            CrosslyDataModel? decompressedDataModel = await compressor.DecompressToDataModel(dataModelStream);

            return !validator.IsValidDataModel(decompressedDataModel)
                ? Results.BadRequest($"dataModel is invalid.")
                : await CreateCreateResult(decompressedDataModel!);
        }

        public async Task<IResult> Replace(string id, Stream dataModelStream)
        {
            CrosslyDataModel? decompressedDataModel = await compressor.DecompressToDataModel(dataModelStream);

            return !validator.IsValidDataModel(decompressedDataModel)
                ? Results.BadRequest($"dataModel is invalid.")
                : await CreateReplaceResult(id, decompressedDataModel!);
        }

        public async Task<IResult> Rename(string id, string newName)
        {
            bool success = await persistence.Rename(id, newName);

            return success
                ? Results.Ok()
                : Results.NotFound();
        }

        public async Task<IResult> Delete(string id)
        {
            bool success = await persistence.Delete(id);

            return success
                ? Results.NoContent()
                : Results.NotFound();
        }

        private async Task<IResult> CreateGetByIdResult(CrosslyDataModel dataModel)
        {
            if (!validator.IsValidDataModel(dataModel))
            {
                throw new Exception($"the retrieved {nameof(dataModel)} is invalid");
            }

            Stream compressedDataModel = await compressor.CompressToStream(dataModel);

            if (!validator.IsValidStream(compressedDataModel))
            {
                throw new Exception($"the {nameof(compressedDataModel)} is invalid");
            }

            compressedDataModel.Seek(0, SeekOrigin.Begin);
            return Results.File(compressedDataModel, "application/octet-stream");
        }

        private async Task<IResult> CreateCreateResult(CrosslyDataModel dataModel)
        {
            string id = await persistence.Create(dataModel);
            string uri = linkGenerator.GetPathByName("GetPatternById", new { id })!;

            return Results.Created(uri, new { id });
        }

        private async Task<IResult> CreateReplaceResult(string id, CrosslyDataModel dataModel)
        {
            bool success = await persistence.Replace(id, dataModel!);

            return success
                ? Results.Ok()
                : Results.NotFound();
        }
    }
}