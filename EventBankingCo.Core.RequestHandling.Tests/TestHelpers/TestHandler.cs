using EventBankingCo.Core.RequestHandling.Abstraction;

namespace EventBankingCo.Core.RequestHandling.Tests.TestHelpers
{
    internal class TestHandler : IHandler<TestRequest, string>
    {
        public Task<string> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(request.ResultValue);
        }
    }
}
