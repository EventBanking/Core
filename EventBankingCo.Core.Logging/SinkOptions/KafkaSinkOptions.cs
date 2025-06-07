using Serilog;
using Serilog.Sinks.Kafka;

namespace EventBankingCo.Core.Logging.Configurations
{
    public class KafkaSinkOptions : BaseSinkOption
    {
        private readonly string _servers;

        private readonly string _topic;

        public KafkaSinkOptions(string servers = "localhost:9092", string topic = "logs", string minLevel = "Information", bool enabled = true) : base(minLevel, enabled)
        {
            _servers = servers;

            _topic = topic;
        }

        public override LoggerConfiguration ConfigureSink(LoggerConfiguration loggerConfig) =>
            loggerConfig.WriteTo.Kafka(
                _ => _topic,
                bootstrapServers: _servers
            );
    }
}