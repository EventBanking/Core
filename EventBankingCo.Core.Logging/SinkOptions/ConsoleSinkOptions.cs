using Serilog;

namespace EventBankingCo.Core.Logging.Configurations
{
    public class ConsoleSinkOptions : BaseSinkOption
    {
        private readonly string _outputTemplate;

        public ConsoleSinkOptions(
            string minLevel = "Debug",
            bool enabled = true,
            string outputTemplate = "[{Timestamp:HH:mm:ss.fff} {Level:u3}] CorrelationId: {CorrelationId} | {Message:lj}{NewLine}{Exception}"
        ) : base(minLevel, enabled)
        {
            _outputTemplate = outputTemplate;
        }

        public override LoggerConfiguration ConfigureSink(LoggerConfiguration loggerConfig) =>
            loggerConfig.WriteTo.Console(
                outputTemplate: _outputTemplate,
                levelSwitch: GetLogLevelSwitch()
            );
    }
}
