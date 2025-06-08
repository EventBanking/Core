using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;

namespace EventBankingCo.Core.RequestHandling.Base
{
    public abstract class RequestHandler<TRequest, TResponse> : CoreHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public RequestHandler(ICoreLoggerFactory loggerFactory) : base(loggerFactory) { }

        protected abstract Task<TResponse> GetResponseAsync(TRequest request, CancellationToken cancellationToken);

        protected override async Task<TResponse> ProcessRequestAsync(TRequest request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Receiving Request", request);

            var response = await GetResponseAsync(request, cancellationToken);

            _logger.LogDebug("Request Handled", response);

            return response;
        }
    }
}
