using EventBankingCo.Core.Logging.Abstraction;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace EventBankingCo.Core.Logging.Configurations
{
    public abstract class BaseSinkOption : ISinkOption
    {
        private readonly LoggingLevelSwitch _logLevelSwitch;

        private readonly bool _enabled;

        public BaseSinkOption(string minLevel, bool enabled = true)
        {
            if(Enum.TryParse<LogEventLevel>(minLevel, ignoreCase: true, out var level))
            {
                _logLevelSwitch = new LoggingLevelSwitch(level);
            }
            else
            {
                throw new ArgumentException($"Invalid log level: {minLevel}", nameof(minLevel));
            }

            _enabled = enabled;
        }

        public abstract LoggerConfiguration ConfigureSink(LoggerConfiguration loggerConfig);

        public LoggingLevelSwitch GetLogLevelSwitch() => _logLevelSwitch;

        public string GetMinLevel() => _logLevelSwitch.MinimumLevel.ToString();

        public bool IsEnabled() => _enabled;
    }

}
