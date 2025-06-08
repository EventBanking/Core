namespace EventBankingCo.Core.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "Object Not Found") : base(message) { }
    }
}
