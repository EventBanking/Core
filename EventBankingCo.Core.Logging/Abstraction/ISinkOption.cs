using Serilog;
using Serilog.Core;

namespace EventBankingCo.Core.Logging.Abstraction
{
    public interface ISinkOption
    {
        public LoggingLevelSwitch GetLogLevelSwitch();
        
        public string GetMinLevel();

        public bool IsEnabled();

        public LoggerConfiguration ConfigureSink(LoggerConfiguration loggerConfig);
    }
}
