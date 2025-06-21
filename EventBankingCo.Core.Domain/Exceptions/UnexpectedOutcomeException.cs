namespace EventBankingCo.Core.Domain.Exceptions
{
    public class UnexpectedOutcomeException : Exception
    {
        public UnexpectedOutcomeException(string message = "Unexpected outcome occurred.") : base(message) { }

        public UnexpectedOutcomeException(string message = "Unexpected outcome occurred.", Exception? innerException = null) : base(message, innerException) { }
    }
}
