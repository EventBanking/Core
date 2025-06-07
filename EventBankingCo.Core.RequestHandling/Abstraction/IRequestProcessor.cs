namespace EventBankingCo.Core.RequestHandling.Abstraction
{
    public interface IRequestProcessor
    {
        Task<TResponse> ProcessRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
