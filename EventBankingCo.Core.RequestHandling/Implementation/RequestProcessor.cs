using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;

namespace EventBankingCo.Core.RequestHandling.Implementation
{
    public class RequestProcessor : IRequestProcessor
    {
        private readonly IHandlerFactory _handlerFactory;

        private readonly ICoreLogger<RequestProcessor> _logger;

        public RequestProcessor(IHandlerFactory handlerFactory, ICoreLogger<RequestProcessor> logger)
        {
            _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ExecuteCommandAsync(ICommand command, CancellationToken cancellationToken = default) =>
            await ProcessAsync<object>(command, cancellationToken);

        public async Task<TResponse> GetResponseAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default) =>
            await ProcessAsync<TResponse>(request, cancellationToken);

        private async Task<TResponse> ProcessAsync<TResponse>(object request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                var ex = new ArgumentNullException(nameof(request), "Request cannot be null.");

                _logger.LogError(ex.Message, ex);

                throw ex;
            }

            var requestType = request.GetType();

            _logger.LogInformation($"Receiving Request: {requestType.Name} for Processing");

            var handler = _handlerFactory.CreateHandler(request);

            _logger.LogInformation($"Found Handler: {handler.GetType().Name} for Request: {requestType.Name}");

            if (handler is IHandler requestHandler)
            {
                var response = await requestHandler.HandleAsync(request, cancellationToken);

                _logger.LogInformation($"Request: {requestType.Name} Processed Successfully, Response Type: {response?.GetType().Name ?? "NULL"}");

                return (TResponse)response!;
            }
            else
            {
                var exception = new InvalidOperationException($"Handler for request type {requestType.Name} does not implement IRequestHandler interface.");

                _logger.LogError(exception.Message, exception);

                throw exception;
            }
        }
    }
}
