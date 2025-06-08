using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Base;

namespace EventBankingCo.Core.RequestHandling.Tests.TestHelpers
{
    public class TestHandler : RequestHandler<TestRequest, string>
    {
        public TestHandler(ICoreLoggerFactory loggerFactory) : base(loggerFactory) { }

        protected override Task<string> GetResponseAsync(TestRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(request.ResultValue);
        }
    }
}
