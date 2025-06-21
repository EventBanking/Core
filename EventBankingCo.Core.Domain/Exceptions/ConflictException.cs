namespace EventBankingCo.Core.Domain.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message = "Conflict occurred") : base(message) { }
        public ConflictException(string message = "Conflict occurred", Exception? innerException = null) : base(message, innerException) { }
    }
}
