namespace EventBankingCo.Core.RequestHandling.Abstraction
{
    public interface IHandler
    {
        Task<object?> HandleAsync(object request, CancellationToken cancellationToken = default);
    }
}
