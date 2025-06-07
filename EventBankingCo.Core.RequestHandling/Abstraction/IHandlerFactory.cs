namespace EventBankingCo.Core.RequestHandling.Abstraction
{
    public interface IHandlerFactory
    {
         public object CreateHandler<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>;
    }
}
