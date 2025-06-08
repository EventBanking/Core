namespace EventBankingCo.Core.RequestHandling.Abstraction
{
    public interface IRequestProcessor
    {
        Task ExecuteCommandAsync(ICommand command, CancellationToken cancellationToken);

        Task <TResponse> GetResponseAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
