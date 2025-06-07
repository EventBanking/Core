using EventBankingCo.Core.Logging.Abstraction;
using Serilog;

namespace EventBankingCo.Core.Logging.Implementation
{
    public class CoreLogger : ICoreLogger
    {
        #region ICoreLogger Implementation

        public void LogTrace(string message, object? extra = null, string memberName = "", string filePath = "") =>
            CreateLog(extra, memberName, filePath).Verbose(message);

        public void LogDebug(string message, object? extra = null, string memberName = "", string filePath = "") =>
            CreateLog(extra, memberName, filePath).Debug(message);

        public void LogInformation(string message, object? extra, string memberName = "", string filePath = "") =>
            CreateLog(extra, memberName, filePath).Information(message);

        public void LogWarning(string message, Exception? ex = null, object? extra = null, string memberName = "", string filePath = "") =>
            CreateLog(extra, memberName, filePath).Warning(ex, message);

        public void LogError(string message, Exception? ex = null, object? extra = null, string memberName = "", string filePath = "") =>
            CreateLog(extra, memberName, filePath).Error(ex, message);

        public void LogCritical(string message, Exception? ex = null, object? extra = null, string memberName = "", string filePath = "") => 
            CreateLog(extra, memberName, filePath).Fatal(ex, message);

        #endregion

        #region Private Helper Methods

        private ILogger CreateLog(object? extra, string memberName, string filePath) =>
            Log.ForContext("ClassName", Path.GetFileNameWithoutExtension(filePath))
               .ForContext("MethodName", memberName)
               .ForContext("Extra", extra);

        #endregion
    }
}
