using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Base;

namespace EventBankingCo.Core.RequestHandling.Tests.TestHelpers
{
    public class TestCommandHandler : CommandHandler<TestCommand>
    {
        public TestCommandHandler(ICoreLoggerFactory logger) : base(logger) { }

        protected override Task ProcessCommandAsync(TestCommand request)
        {
            _logger.LogTrace(request.ValueToTrace);

            return Task.CompletedTask;
        }
    }
}
