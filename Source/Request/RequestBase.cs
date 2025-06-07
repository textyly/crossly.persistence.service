
namespace Persistence.Request
{
    public abstract class RequestBase<TInput>(TInput input) : IRequest
    {
        public Task<IResult> Execute()
        {
            // first validate client input
            IResult? error = Validate(input);

            return error is not null
                ? Task.FromResult(error)    // notify client that the input is invalid
                : Execute(input);           // execute client request
        }

        protected abstract IResult? Validate(TInput input);
        protected abstract Task<IResult> ExecuteRequest(TInput input);

        private async Task<IResult> Execute(TInput input)
        {
            // TODO: is it ok to have error handling here? We know that errors here are internal
            // but should we handle them here or on a higher level???
            try
            {
                // TODO: add metric
                return await ExecuteRequest(input);
            }
            catch
            {
                // TODO: add log and metric as well as some text to the exception
                return Results.InternalServerError();
            }
        }
    }
}