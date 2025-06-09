using EventBankingCo.Core.Logging.Abstraction;

namespace EventBankingCo.Core.Logging.Implementation
{
    public class CoreLoggerFactory : ICoreLoggerFactory
    {
        public ICoreLogger<T> Create<T>() => new CoreLogger<T>();

        public ICoreLogger<T> Create<T>(T t) => Create<T>();
    }
}
