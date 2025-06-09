using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.Logging.Configurations;

namespace EventBankingCo.Core.ApiShared.Startup
{
    public class ApiSharedConfigs
    {
        public string ServiceName { get; set; } = string.Empty;

        public ISinkOption[] LoggingSinkOptions { get; set; } = [];

        public string DatabaseConnectionString { get; set; } = string.Empty;

        public ApiSharedConfigs()
        {
            
        }

        public ApiSharedConfigs(string serviceName, string databaseConnectionString, ISinkOption[] loggingSinkOptions)
        {
            ServiceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName), "Service name cannot be null");
            LoggingSinkOptions = loggingSinkOptions ?? throw new ArgumentNullException(nameof(loggingSinkOptions), "Logging sink options cannot be null");
            DatabaseConnectionString = databaseConnectionString ?? throw new ArgumentNullException(nameof(databaseConnectionString), "Database connection string cannot be null");
        }

        public ApiSharedConfigs(string serviceName, string databaseConnectionString, string kafkaSinkServer = "localhost:29092")
            : this (serviceName, databaseConnectionString, [
                new ConsoleSinkOptions(minLevel: "Debug"),
                new FileSinkOptions(),
                new KafkaSinkOptions(servers: kafkaSinkServer, enabled: false),
                new SqlServerDatabaseSinkOptions(databaseConnectionString)
            ])
        {

        }
    }
}
