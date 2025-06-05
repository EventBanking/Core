using EventBankingCo.Core.Logging.Abstraction;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace EventBankingCo.Core.Logging.Implementation
{
    public class CoreLogger : ICoreLogger
    {
        #region Private Readonly Fields

        private readonly ILogger<CoreLogger> _logger;

        #endregion

        #region Constructor

        public CoreLogger(ILogger<CoreLogger> logger)
        {
            _logger = logger;
        }

        #endregion

        #region ICoreLogger Implementation

        public void LogTrace(string message, string memberName = "", string filePath = "") =>
            _logger.LogTrace("TraceId: {TraceId} | Class: {ClassName} | Method: {MemberName} | {Message}",
                GetTraceId(), GetFileName(filePath), memberName, message);

        public void LogDebug(string message, string memberName = "", string filePath = "") =>
            _logger.LogDebug("TraceId: {TraceId} | Class: {ClassName} | Method: {MemberName} | {Message}",
                GetTraceId(), GetFileName(filePath), memberName, message);

        public void LogInformation(string message, string memberName = "", string filePath = "") =>
            _logger.LogInformation("TraceId: {TraceId} | Class: {ClassName} | Method: {MemberName} | {Message}",
                GetTraceId(), GetFileName(filePath), memberName, message);

        public void LogWarning(string message, Exception? ex = null, string memberName = "", string filePath = "") =>
            _logger.LogWarning(ex, "TraceId: {TraceId} | Class: {ClassName} | Method: {MemberName} | {Message}",
                GetTraceId(), GetFileName(filePath), memberName, message);

        public void LogError(string message, Exception? ex = null, string memberName = "", string filePath = "") => 
            _logger.LogError(ex, "TraceId: {TraceId} | Class: {ClassName} | Method: {MemberName} | {Message}",
                GetTraceId(), GetFileName(filePath), memberName, message);

        public void LogCritical(string message, Exception? ex = null, string memberName = "", string filePath = "") => 
            _logger.LogCritical(ex, "TraceId: {TraceId} | Class: {ClassName} | Method: {MemberName} | {Message}",
                GetTraceId(), GetFileName(filePath), memberName, message);
    
        #endregion

        #region Private Helper Methods

        private static string GetTraceId() => Activity.Current?.TraceId.ToString() ?? "N/A";

        private static string GetFileName(string filePath) => Path.GetFileNameWithoutExtension(filePath);

        #endregion
    }
}
