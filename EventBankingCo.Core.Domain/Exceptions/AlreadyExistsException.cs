namespace EventBankingCo.Core.Domain.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string message = "Object Already Exists") : base(message) { }
        public AlreadyExistsException(string message = "Object Already Exists", Exception? innerException = null) : base(message, innerException) { }
    }
}
