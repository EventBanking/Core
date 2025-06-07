using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace EventBankingCo.Core.Logging.Implementation
{
    internal class ClassEnrichedLogger<T> : ILogger<T>
    {
        private readonly ILogger _logger;

        public ClassEnrichedLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(T));
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => _logger.BeginScope(state);

        public bool IsEnabled(LogLevel logLevel) => _logger.IsEnabled(logLevel);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            // Push ClassName into the Serilog context for this log
            using (LogContext.PushProperty("ClassName", typeof(T).Name))
            {
                _logger.Log(logLevel, eventId, state, exception, formatter);
            }
        }
    }
}
