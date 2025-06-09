namespace EventBankingCo.Core.Logging.Abstraction
{
    public interface ICoreLoggerFactory
    {
        ICoreLogger<T> Create<T>();

        ICoreLogger<T> Create<T>(T t);
    }
}
