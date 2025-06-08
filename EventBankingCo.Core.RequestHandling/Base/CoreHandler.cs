using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;

namespace EventBankingCo.Core.RequestHandling.Base
{
    public abstract class CoreHandler<TRequest, TResponse> : IRequestHandler<TRequest>
    {
        protected ICoreLogger<CoreHandler<TRequest, TResponse>> _logger;

        protected CoreHandler(ICoreLoggerFactory loggerFactory)
        {
            _logger = loggerFactory.Create(this);
        }

        protected abstract Task<TResponse> ProcessRequestAsync(TRequest request, CancellationToken cancellationToken);

        public async Task<object?> HandleAsync(object request, CancellationToken cancellationToken = default)
        {
            if(request is null)
            {
                var exception = new ArgumentNullException(nameof(request), "Request cannot be null.");

                _logger.LogError(exception.Message, exception);

                throw exception;
            }

            if (request is not TRequest typedRequest)
            {
                var exception = new ArgumentException($"Invalid request type. Expected {typeof(TRequest).Name}, but received {request.GetType().Name}.", nameof(request));

                _logger.LogError(exception.Message, exception);

                throw exception;
            }

            var requestName = typedRequest.GetType().Name;
            var requestFullName = typedRequest.GetType().FullName;

            _logger.LogInformation($"Received Request: {requestName}", requestFullName);

            var response = await ProcessRequestAsync(typedRequest, cancellationToken);

            var responseName = response?.GetType().Name ?? "NULL";
            var responseFullName = response?.GetType().FullName ?? "NULL";

            _logger.LogInformation($"Handled Request: {requestName} with Response: {responseName ?? "NULL"}", 
                extra: new { RequestType = requestFullName, ResponseType = responseFullName });

            return response;
        }
    }
}
