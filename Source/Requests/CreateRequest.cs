
using Persistence.DataModel;
using Persistence.Repository;
using Persistence.Validation;
using Persistence.Compression;

namespace Persistence.Requests
{
    public class CreateRequest : IRequest
    {
        private readonly IRepository repository;
        private readonly IValidator validator;
        private readonly ICompressor compressor;

        private readonly Stream? dataModelStream;
        private readonly IResult? error;

        public CreateRequest(CreateInput input, IRepository repository, IValidator validator, ICompressor compressor)
        {
            this.repository = repository;
            this.validator = validator;
            this.compressor = compressor;

            // TODO: export to validator
            if (validator.ValidateStream(input.DataModelStream, out error))
            {
                dataModelStream = input.DataModelStream;
            }
        }

        public async Task<IResult> Execute() => error ?? await Execute(dataModelStream!);

        // TODO: refactor!!!
        private async Task<IResult> Execute(Stream dataModelStream)
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

            if (!validator.ValidateDataModel(dataModel, out IResult? error))
            {
                // TODO: log and send metric
                return error!;
            }

            // all good, return to the client
            // TODO: send metric
            return await repository.Create(dataModel!);
        }
    }

    public record CreateInput(Stream DataModelStream);
}