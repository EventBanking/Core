using EventBankingCo.Core.Logging.Abstraction;

namespace EventBankingCo.Core.Logging.Implementation
{
    public class CoreLoggerFactory : ICoreLoggerFactory
    {
        public ICoreLogger<T> Create<T>(T type) => new CoreLogger<T>();
    }
}
