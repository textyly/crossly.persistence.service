
using Persistence.DataModel;
using Persistence.Repository;
using Persistence.Validation;
using Persistence.Compression;

namespace Persistence.Requests
{
    public class GetByIdRequest : IRequest
    {
        private readonly IRepository repository;
        private readonly IValidator validator;
        private readonly ICompressor compressor;

        private readonly string? id;
        private readonly IResult? error;

        public GetByIdRequest(GetByIdInput input, IRepository repository, IValidator validator, ICompressor compressor)
        {
            this.repository = repository;
            this.validator = validator;
            this.compressor = compressor;

            if (validator.ValidateId(input.Id, out error))
            {
                id = input.Id;
            }
        }

        public async Task<IResult> Execute() => error ?? await Execute(id!);

        // TODO: Refactor!!!
        private async Task<IResult> Execute(string id)
        {
            CrosslyDataModel? dataModel = await repository.GetById(id);

            if (!validator.ValidateDataModel(dataModel, out IResult? error))
            {
                // TODO: log and send metric
                return error!;
            }

            Stream? dataModelStream;
            try
            {
                dataModelStream = await compressor.CompressToStream(dataModel!);
            }
            catch (Exception e)
            {
                // TODO: log and send metric
                return Results.InternalServerError();
            }

            if (!validator.ValidateStream(dataModelStream, out error))
            {
                // TODO: log and send metric
                return error!;
            }

            // all good, return to the client
            // TODO: send metric
            dataModelStream!.Seek(0, SeekOrigin.Begin);
            return Results.File(dataModelStream, "application/octet-stream");
        }
    }

    public record GetByIdInput(string Id);
}