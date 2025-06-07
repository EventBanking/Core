using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;
using System.Collections.Concurrent;

namespace EventBankingCo.Core.RequestHandling.Implementation
{
    public class RequestProcessor : IRequestProcessor
    {
        private readonly IHandlerFactory _handlerFactory;
        private readonly ICoreLogger _logger;

        // Static cache of compiled delegates
        private static readonly ConcurrentDictionary<Type, Func<object, object, CancellationToken, Task<object>>> _handlerInvokerCache = new();

        public RequestProcessor(IHandlerFactory handlerFactory, ICoreLogger logger)
        {
            _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ProcessCommandAsync(ICommand command, CancellationToken cancellationToken) =>
            await ProcessRequestAsync(command, cancellationToken);

        public async Task<TResponse> ProcessRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                var ex = new ArgumentNullException(nameof(request), "Request cannot be null.");

                _logger.LogError(ex.Message, ex);
                
                throw ex;
            }

            var requestType = request.GetType();

            _logger.LogInformation($"Receiving Request: {requestType.Name} for Processing");

            var handler = _handlerFactory.CreateHandler<IRequest<TResponse>, TResponse>(request);

            if (handler is null)
            {
                var exception = new InvalidOperationException($"No handler found for request type {requestType.Name}.");

                _logger.LogError(exception.Message, exception);

                throw exception;
            }

            _logger.LogInformation($"Found Handler: {handler.GetType().Name} for Request: {requestType.Name}");

            var cacheKey = requestType;

            if (!_handlerInvokerCache.TryGetValue(cacheKey, out var invoker))
            {
                _logger.LogDebug("Handler delegate not found in cache. Building new one.");

                invoker = BuildHandlerDelegate(handler.GetType(), requestType, typeof(TResponse));

                _handlerInvokerCache.TryAdd(cacheKey, invoker);
            }
            try
            {
                var response = await invoker.Invoke(handler, request, cancellationToken);

                _logger.LogInformation($"Request: {requestType.Name} processed successfully");

                return (TResponse)response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while processing request: {requestType.Name}", ex);
                throw;
            }
        }

        private Func<object, object, CancellationToken, Task<object>> BuildHandlerDelegate(Type handlerType, Type requestType, Type responseType)
        {
            var interfaceType = typeof(IHandler<,>).MakeGenericType(requestType, responseType);
            var method = interfaceType.GetMethod("HandleAsync");

            if (method is null)
                throw new InvalidOperationException($"HandleAsync not found on {interfaceType.Name}");

            return async (handler, request, cancellationToken) =>
            {
                // Safe cast using reflection
                if (!interfaceType.IsInstanceOfType(handler))
                {
                    throw new InvalidCastException($"Handler does not implement expected interface {interfaceType.Name}");
                }

                // Call method via reflection
                var resultTask = method.Invoke(handler, new object[] { request, cancellationToken }) as Task;

                if (resultTask is null)
                {
                    throw new InvalidOperationException($"HandleAsync did not return a Task for {handlerType.Name}");
                }

                // Get Task<T>.Result
                var resultProperty = resultTask.GetType().GetProperty("Result");

                return resultProperty?.GetValue(resultTask)!;
            };
        }

    }
}
