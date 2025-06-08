namespace EventBankingCo.Core.Domain.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string message = "Object Already Exists") : base(message) { }
    }
}
