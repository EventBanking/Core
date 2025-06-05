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
            _logger.LogTrace(message, "{MemberName} | Class: {ClassName} | TraceId: {TraceId}",
                memberName, GetFileName(filePath), GetTraceId());

        public void LogDebug(string message, string memberName = "", string filePath = "") =>
            _logger.LogDebug("{Message} | Method: {MemberName} | Class: {ClassName} | TraceId: {TraceId}",
                message, memberName, GetFileName(filePath), GetTraceId());
        public void LogInformation(string message, string memberName = "", string filePath = "") => 
            _logger.LogInformation("{Message} | Method: {MemberName} | Class: {ClassName} | TraceId: {TraceId}",
                message, memberName, GetFileName(filePath), GetTraceId());

        public void LogWarning(string message, Exception? ex = null, string memberName = "", string filePath = "") => 
            _logger.LogWarning(ex, "{Message} | Method: {MemberName} | Class: {ClassName} | TraceId: {TraceId}",
                message, memberName, GetFileName(filePath), GetTraceId());

        public void LogError(string message, Exception? ex = null, string memberName = "", string filePath = "") => 
            _logger.LogError(ex, "{Message} | Method: {MemberName} | Class: {ClassName} | TraceId: {TraceId}",
                message, memberName, GetFileName(filePath), GetTraceId());

        public void LogCritical(string message, Exception? ex = null, string memberName = "", string filePath = "") => 
            _logger.LogCritical(ex, "{Message} | Method: {MemberName} | Class: {ClassName} | TraceId: {TraceId}",
                message, memberName, GetFileName(filePath), GetTraceId());
    
        #endregion

        #region Private Helper Methods

        private static string GetTraceId() => Activity.Current?.TraceId.ToString() ?? "N/A";

        private static string GetFileName(string filePath) => Path.GetFileNameWithoutExtension(filePath);

        #endregion
    }
}
