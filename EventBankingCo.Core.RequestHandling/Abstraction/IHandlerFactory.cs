namespace EventBankingCo.Core.RequestHandling.Abstraction
{
    public interface IHandlerFactory
    {
         public object CreateHandler<TRequest>(TRequest request);
    }
}
