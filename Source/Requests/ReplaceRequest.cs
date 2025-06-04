using Persistence.DataModel;
using Persistence.Repository;
using Persistence.Validation;
using Persistence.Compression;

namespace Persistence.Requests
{
    public class ReplaceRequest : IRequest
    {
        private readonly IRepository repository;
        private readonly IValidator validator;
        private readonly ICompressor compressor;

        private readonly string? id;
        private readonly Stream? dataModelStream;
        private readonly IResult? error;

        public ReplaceRequest(ReplaceInput input, IRepository repository, IValidator validator, ICompressor compressor)
        {
            this.repository = repository;
            this.validator = validator;
            this.compressor = compressor;

            if (validator.ValidateId(input.Id, out error))
            {
                id = input.Id;
            }

            if (validator.ValidateStream(input.DataModelStream, out error))
            {
                dataModelStream = input.DataModelStream;
            }
        }

        public async Task<IResult> Execute() => error ?? await Execute(id!, dataModelStream!);

        private async Task<IResult> Execute(string id, Stream dataModelStream)
        {
            CrosslyDataModel? dataModel;

            try
            {
                dataModel = await compressor.TryDecompressToDataModel(dataModelStream);
            }
            catch (Exception e)
            {
                // TODO: log and send metric
                return Results.BadRequest($"http body is not valid.");
            }

            return !validator.ValidateDataModel(dataModel, out IResult? error)
                ? error!
                : await Execute(id, dataModel!);
        }

        private async Task<IResult> Execute(string id, CrosslyDataModel dataModel)
        {
            if (!validator.ValidateDataModel(dataModel, out IResult? error))
            {
                // TODO: log and send metric
                return error!;
            }

            // all good, return to the client
            // TODO: send metric
            return await repository.Replace(id, dataModel!);
        }
    }

    public record ReplaceInput(string Id, Stream DataModelStream);
}