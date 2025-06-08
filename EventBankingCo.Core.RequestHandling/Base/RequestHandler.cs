using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;

namespace EventBankingCo.Core.RequestHandling.Base
{
    public abstract class RequestHandler<TRequest, TResponse> : IHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected readonly ICoreLogger<RequestHandler<TRequest,TResponse>> _logger;

        protected RequestHandler(ICoreLoggerFactory loggerFactory)
        {
            _logger = loggerFactory.Create(this);
        }

        protected abstract Task<TResponse> ProcessRequestAsync(TRequest request, CancellationToken cancellationToken);

        public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                var exception = new ArgumentNullException(nameof(request), "Request cannot be null.");

                _logger.LogError(exception.Message, exception);

                throw exception;
            }

            _logger.LogInformation($"Handling Request: {request.GetType().Name}");

            var response = await ProcessRequestAsync(request, cancellationToken);

            _logger.LogInformation($"Handled Request: {request.GetType().Name} with Response: {response?.GetType().Name}");

            return response;
        }
    }
}
