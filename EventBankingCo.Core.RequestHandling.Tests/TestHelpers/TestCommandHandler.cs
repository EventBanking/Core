using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Base;

namespace EventBankingCo.Core.RequestHandling.Tests.TestHelpers
{
    public class TestCommandHandler : CommandHandler<TestCommand>
    {
        public TestCommandHandler(ICoreLoggerFactory logger) : base(logger) { }

        protected override Task ExecuteCommandAsync(TestCommand request, CancellationToken cancellationToken)
        {
            _logger.LogTrace(request.ValueToTrace);

            return Task.CompletedTask;
        }
    }
}
