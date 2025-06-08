using System.Runtime.CompilerServices;

namespace EventBankingCo.Core.Logging.Abstraction
{
    public interface ICoreLogger<T>
    {
        void LogTrace(string message, object? extra = null, [CallerMemberName] string memberName = "");

        void LogDebug(string message, object? extra = null, [CallerMemberName] string memberName = "");

        void LogInformation(string message, object? extra = null, [CallerMemberName] string memberName = "");

        void LogWarning(string message, Exception? ex = null, object? extra = null, [CallerMemberName] string memberName = "");

        void LogError(string message, Exception? ex = null, object? extra = null, [CallerMemberName] string memberName = "");

        void LogCritical(string message, Exception? ex = null, object? extra = null, [CallerMemberName] string memberName = "");
    }
}
