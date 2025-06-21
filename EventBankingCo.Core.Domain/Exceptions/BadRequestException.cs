namespace EventBankingCo.Core.Domain.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { } 
        public BadRequestException(string message, Exception? innerException = null) : base(message, innerException) { }
    }
}
