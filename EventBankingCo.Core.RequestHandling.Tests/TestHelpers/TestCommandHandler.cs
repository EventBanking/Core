using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Base;

namespace EventBankingCo.Core.RequestHandling.Tests.TestHelpers
{
    public class TestCommandHandler : CommandHandler<TestCommand>
    {
        private readonly ICoreLogger<TestCommandHandler> _logger;

        public TestCommandHandler(ICoreLoggerFactory loggerFactory) : base(loggerFactory) => _logger = loggerFactory.Create(this);

        protected override Task ExecuteCommandAsync(TestCommand request, CancellationToken cancellationToken)
        {
            _logger.LogTrace(request.ValueToTrace);

            return Task.CompletedTask;
        }
    }
}
