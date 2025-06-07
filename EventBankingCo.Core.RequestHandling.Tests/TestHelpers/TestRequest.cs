using EventBankingCo.Core.RequestHandling.Abstraction;

namespace EventBankingCo.Core.RequestHandling.Tests.TestHelpers
{
    class TestRequest : IRequest<string>
    {
        public TestRequest(string value = "TestValue")
        {
            ResultValue = value;
        }

        public string ResultValue { get; set; }
    }
}
