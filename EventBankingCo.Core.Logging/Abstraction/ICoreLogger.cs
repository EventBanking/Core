using System.Runtime.CompilerServices;

namespace EventBankingCo.Core.Logging.Abstraction
{
    public interface ICoreLogger
    {
        void LogTrace(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "");

        void LogDebug(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "");

        void LogInformation(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "");

        void LogWarning(string message, Exception? ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "");

        void LogError(string message, Exception? ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "");

        void LogCritical(string message, Exception? ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "");
    }

}
