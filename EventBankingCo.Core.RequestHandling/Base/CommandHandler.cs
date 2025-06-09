using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;

namespace EventBankingCo.Core.RequestHandling.Base
{
    public abstract class CommandHandler<TRequest> : CoreHandler<TRequest, object?> where TRequest : ICommand
    {
        private readonly ICoreLogger<CommandHandler<TRequest>> _logger;

        protected CommandHandler(ICoreLoggerFactory loggerFactory) : base(loggerFactory) => _logger = loggerFactory.Create<CommandHandler<TRequest>>();

        protected abstract Task ExecuteCommandAsync(TRequest request, CancellationToken cancellationToken);

        protected override async Task<object?> ProcessRequestAsync(TRequest request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Executing Command", request);

            await ExecuteCommandAsync(request, cancellationToken);

            _logger.LogDebug($"Command Executed Successfully");

            return Task.CompletedTask;
        }
    }
}
