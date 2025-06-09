using Serilog;

namespace EventBankingCo.Core.Logging.Configurations
{
    public class FileSinkOptions : BaseSinkOption
    {
        private readonly string _path;

        private readonly RollingInterval _rollingInterval;

        private readonly string _outputTemplate;

        public FileSinkOptions(
            string minLevel = "Information", 
            bool enabled = true,
            string path = "logs/log-.txt",
            RollingInterval rollingInterval = RollingInterval.Day,
            string outputTemplate = "[{Timestamp:HH:mm:ss.fff} {Level:u3}] CorrelationId: {CorrelationId} | {Message:lj}{NewLine}{Exception}"
            ) : base(minLevel, enabled)
        {
            _path = path;
            _rollingInterval = rollingInterval;
            _outputTemplate = outputTemplate;
        }

        public override LoggerConfiguration ConfigureSink(LoggerConfiguration loggerConfig) =>
            loggerConfig.WriteTo.File(
                _path,
                outputTemplate: _outputTemplate,
                rollingInterval: _rollingInterval,
                levelSwitch: GetLogLevelSwitch()
            );
    }

}
