namespace EventBankingCo.Core.RequestHandling.Abstraction
{
    public interface IHandlerDictionary
    {
        Type GetHandlerType(Type requestType);
    }
}
