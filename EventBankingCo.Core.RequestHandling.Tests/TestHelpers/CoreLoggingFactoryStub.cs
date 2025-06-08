using EventBankingCo.Core.Logging.Abstraction;
using Moq;

namespace EventBankingCo.Core.RequestHandling.Tests.TestHelpers
{
    internal class CoreLoggingFactoryStub : ICoreLoggerFactory
    {
        public ICoreLogger<T> Create<T>(T type)
        {
            return new Mock<ICoreLogger<T>>().Object;
        }
    }
}
