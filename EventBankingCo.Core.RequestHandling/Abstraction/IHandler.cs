namespace EventBankingCo.Core.RequestHandling.Abstraction
{
    internal interface IHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
    }

}
