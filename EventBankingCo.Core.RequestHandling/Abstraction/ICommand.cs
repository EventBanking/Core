namespace EventBankingCo.Core.RequestHandling.Abstraction
{
    public interface IRequest<TResponse> { }

    public interface ICommand : IRequest<object?>
    {
    }
}
