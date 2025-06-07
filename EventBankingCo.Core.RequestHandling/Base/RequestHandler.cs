using Azure;
using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;

namespace EventBankingCo.Core.RequestHandling.Base
{
    public abstract class RequestHandler<TRequest, TResponse> : IHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected readonly ICoreLogger _logger;

        protected RequestHandler(ICoreLogger logger)
        {
            _logger = logger;
        }

        protected abstract Task<TResponse> ProcessRequestAsync(TRequest request);

        public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                var exception = new ArgumentNullException(nameof(request), "Request cannot be null.");

                _logger.LogError(exception.Message, exception);

                throw exception;
            }

            _logger.LogInformation($"Handling Request: {request.GetType().Name}");

            var response = await ProcessRequestAsync(request);

            _logger.LogInformation($"Handled Request: {request.GetType().Name} with Response: {response?.GetType().Name}");

            return response;
        }
    }

    public abstract class CommandHandler<TRequest> : IHandler<TRequest, object?> where TRequest : ICommand
    {
        protected readonly ICoreLogger _logger;

        protected CommandHandler(ICoreLogger logger)
        {
            _logger = logger;
        }

        protected abstract Task ProcessCommandAsync(TRequest request);

        public async Task<object?> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                var exception = new ArgumentNullException(nameof(request), "Request cannot be null.");

                _logger.LogError(exception.Message, exception);

                throw exception;
            }

            _logger.LogInformation($"Handling Request: {request.GetType().Name}");

            await ProcessCommandAsync(request);

            _logger.LogInformation($"Handled Request: {request.GetType().Name}");

            return null;
        }
    }
}
