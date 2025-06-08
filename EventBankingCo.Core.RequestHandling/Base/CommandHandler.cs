using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;

namespace EventBankingCo.Core.RequestHandling.Base
{
    public abstract class CommandHandler<TRequest> : IHandler<TRequest, object?> where TRequest : ICommand
    {
        protected readonly ICoreLogger<CommandHandler<TRequest>> _logger;

        protected CommandHandler(ICoreLoggerFactory loggerFactory)
        {
            var logger = loggerFactory.Create(this);

            _logger = logger;
        }

        protected abstract Task ProcessCommandAsync(TRequest request, CancellationToken cancellationToken);

        public async Task<object?> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                var exception = new ArgumentNullException(nameof(request), "Request cannot be null.");

                _logger.LogError(exception.Message, exception);

                throw exception;
            }

            _logger.LogInformation($"Handling Request: {request.GetType().Name}");

            await ProcessCommandAsync(request, cancellationToken);

            _logger.LogInformation($"Handled Request: {request.GetType().Name}");

            return null;
        }
    }
}
