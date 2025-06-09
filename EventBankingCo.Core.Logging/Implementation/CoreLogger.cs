using EventBankingCo.Core.Logging.Abstraction;
using Serilog;

namespace EventBankingCo.Core.Logging.Implementation
{
    public class CoreLogger<T> : ICoreLogger<T>
    {
        private readonly string? _sourceContext;

        private readonly string? _namespace;

        private readonly string? _className;

        public CoreLogger(T t)
        {
            var type = t?.GetType();

            _sourceContext = type?.FullName;
            _namespace = type?.Namespace;
            _className = type?.Name;
        }

        #region ICoreLogger Implementation

        public void LogTrace(string message, object? extra = null, string memberName = "") =>
            CreateLog(extra, memberName).Verbose(message);

        public void LogDebug(string message, object? extra = null, string memberName = "") =>
            CreateLog(extra, memberName).Debug(message);

        public void LogInformation(string message, object? extra, string memberName = "") =>
            CreateLog(extra, memberName).Information(message);

        public void LogWarning(string message, Exception? ex = null, object? extra = null, string memberName = "") =>
            CreateLog(extra, memberName).Warning(ex, message);

        public void LogError(string message, Exception? ex = null, object? extra = null, string memberName = "") =>
            CreateLog(extra, memberName).Error(ex, message);

        public void LogCritical(string message, Exception? ex = null, object? extra = null, string memberName = "") => 
            CreateLog(extra, memberName).Fatal(ex, message);

        #endregion

        #region Private Helper Methods

        private ILogger CreateLog(object? extra, string memberName) =>
            Log.ForContext("SourceContext", typeof(T).FullName)
               .ForContext("Namespace", typeof(T).Namespace)
               .ForContext("ClassName", typeof(T).Name)
               .ForContext("MethodName", memberName)
               .ForContext("Extra", extra);

        #endregion
    }
}
