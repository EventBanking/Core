using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace EventBankingCo.Core.RequestHandling.Implementation
{
    public class RequestProcessor : IRequestProcessor
    {
        private readonly IHandlerFactory _handlerFactory;
        private readonly ICoreLogger _logger;

        // Static cache of compiled delegates
        private static readonly ConcurrentDictionary<Type, Func<object, object, CancellationToken, Task<object>>> _handlerInvokerCache = new();

        public RequestProcessor(IHandlerFactory handlerFactory, ICoreLogger logger, IHandlerDictionary handlerDictionary)
        {
            _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            PreloadAllHandlers(handlerDictionary);
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

        public void PreloadAllHandlers(IHandlerDictionary handlerDictionary)
        {
            foreach (var kvp in handlerDictionary.GetDictionary())
            {
                var requestType = kvp.Key;
                var handlerType = kvp.Value;

                var interfaceType = handlerType
                    .GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandler<,>));

                if (interfaceType == null)
                    continue;

                var responseType = interfaceType.GetGenericArguments()[1];

                var cacheKey = requestType;

                if (!_handlerInvokerCache.ContainsKey(cacheKey))
                {
                    var invoker = BuildHandlerDelegate(handlerType, requestType, responseType);
                    _handlerInvokerCache.TryAdd(cacheKey, invoker);
                }
            }

            _logger.LogInformation($"Preloaded {_handlerInvokerCache.Count} handler delegates.");
        }

        #region Private Helper Methods - Building Handler Delegate

        private Func<object, object, CancellationToken, Task<object>> BuildHandlerDelegate(Type handlerType, Type requestType, Type responseType)
        {
            var interfaceType = typeof(IHandler<,>).MakeGenericType(requestType, responseType);
            var methodInfo = interfaceType.GetMethod("HandleAsync");
            if (methodInfo == null)
                throw new InvalidOperationException($"HandleAsync not found on {interfaceType.Name}");

            // Parameters to the lambda: (object handler, object request, CancellationToken cancellationToken)
            var handlerParam = Expression.Parameter(typeof(object), "handler");
            var requestParam = Expression.Parameter(typeof(object), "request");
            var tokenParam = Expression.Parameter(typeof(CancellationToken), "cancellationToken");

            // Cast parameters to their actual types
            var castedHandler = Expression.Convert(handlerParam, interfaceType);
            var castedRequest = Expression.Convert(requestParam, requestType);

            // Call HandleAsync(castedRequest, cancellationToken)
            var call = Expression.Call(castedHandler, methodInfo, castedRequest, tokenParam);

            // Convert Task<T> to Task<object>
            var taskOfResponse = typeof(Task<>).MakeGenericType(responseType);
            var resultProperty = taskOfResponse.GetProperty("Result");

            // async lambda equivalent:
            // return await ((Task<T>)call);
            var lambda = Expression.Lambda<Func<object, object, CancellationToken, Task<object>>>(
                Expression.Call(
                    typeof(RequestProcessor),
                    nameof(CastToTaskObject),
                    new[] { responseType },
                    call
                ),
                handlerParam,
                requestParam,
                tokenParam
            );

            return lambda.Compile();
        }

        private static async Task<object> CastToTaskObject<T>(Task<T> task)
        {
            return await task.ConfigureAwait(false)! ?? default!;
        }


        static class TaskConverter
        {
            public static async Task<object> ConvertTaskResult<T>(Task<T> task) => await task.ConfigureAwait(false)! ?? default!;
        }

        #endregion
    }
}
