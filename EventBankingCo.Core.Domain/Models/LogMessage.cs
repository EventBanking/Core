namespace EventBankingCo.Core.Domain.Models
{
    public class LogMessage
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string Level { get; set; } = string.Empty;

        public string? ServiceName { get; set; }
        
        public string? SourceContext { get; set; }

        public string? MethodName { get; set; }

        public string? TraceId { get; set; }

        public string? CorrelationId { get; set; }

        public string? Message { get; set; } = string.Empty;
        
        public string? Extra { get; set; } = string.Empty;

        public string? ExceptionType { get; set; }

        public string? ExceptionMessage { get; set; }

        public string? Exception { get; set; }

        public string? StackTrace { get; set; }

        public string? MessageTemplate { get; set; }

        public string? Properties { get; set; }
    }
}
