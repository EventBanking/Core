using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;

namespace EventBankingCo.Core.RequestHandling.Implementation
{
    public class HandlerFactory : IHandlerFactory
    {
        #region Private Members

        private readonly IHandlerDictionary _handlerDictionary;

        private readonly ITypeInstantiator _typeActivator;

        private readonly ICoreLogger<HandlerFactory> _logger;

        #endregion

        #region Constructor

        public HandlerFactory(ITypeInstantiator typeActivator, IHandlerDictionary handlers, ICoreLoggerFactory loggerFactory)
        {
            _typeActivator = typeActivator;

            _handlerDictionary = handlers;

            _logger = loggerFactory.Create(this);
        }

        #endregion

        #region Public Method

        public object CreateHandler<TRequest>(TRequest request)
        {
            if (request is null)
            {
                var exception = new ArgumentNullException(nameof(request));

                throw exception;
            }

            var requestType = request.GetType();

            _logger.LogInformation("Handler Factory Receiving Request", requestType.Name);

            var handlerType = _handlerDictionary.GetHandlerType(requestType);

            if(handlerType is null)
            {
                var exception = new InvalidOperationException($"No handler found for request type {requestType.Name}.");

                _logger.LogError(exception.Message, exception);

                throw exception;
            }

            _logger.LogDebug($"Located Handler Type: {handlerType.Name} for Request: {requestType.Name}");

            var handler = _typeActivator.Instantiate(handlerType);

            if (handler is null)
            {
                var exception = new InvalidOperationException($"Handler for request type {requestType.Name} could not be instantiated.");

                _logger.LogError(exception.Message, exception);

                throw exception;
            }

            _logger.LogInformation($"Instantiated Handler: {handler.GetType().Name} for Request: {request.GetType().Name}");

            return handler;
        }

        #endregion
    }
}
