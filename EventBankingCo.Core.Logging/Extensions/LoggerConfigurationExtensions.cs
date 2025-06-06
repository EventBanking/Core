using EventBankingCo.Core.Logging.Configurations;
using EventBankingCo.Core.Logging.Enrichers;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace EventBankingCo.Core.Logging.Extensions
{
    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration ApplyCoreConfiguration(
            this LoggerConfiguration config,
            LoggingOptions options,
            IConfiguration configuration,
            out Dictionary<string, LoggingLevelSwitch> sinkLevelSwitches)
        {
            var consoleSwitch = new LoggingLevelSwitch(ParseLogLevel(options.Console.MinLevel));
            var fileSwitch = new LoggingLevelSwitch(ParseLogLevel(options.File.MinLevel));
            var dbSwitch = new LoggingLevelSwitch(ParseLogLevel(options.Database.MinLevel));

            sinkLevelSwitches = new()
            {
                { "Console", consoleSwitch },
                { "File", fileSwitch },
                { "Database", dbSwitch }
            };

            config.ReadFrom.Configuration(configuration)
                  .Enrich.FromLogContext()
                  .Enrich.With(new LoggingContextEnricher())

            if (options.Console.Enabled)
            {
                config.WriteTo.Logger(lc => lc
                    .MinimumLevel.ControlledBy(consoleSwitch)
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] ({CorrelationId}) {Message:lj}{NewLine}{Exception}"));

            }

            if (options.File.Enabled)
            {
                config.WriteTo.Logger(lc => lc
                    .MinimumLevel.ControlledBy(fileSwitch)
                    .WriteTo.File(
                        options.File.Path,
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] ({CorrelationId}) {Message:lj}{NewLine}{Exception}",
                        rollingInterval: RollingInterval.Day));
            }

            if (options.Database.Enabled)
            {
                config.WriteTo.Logger(lc => lc
                    .MinimumLevel.ControlledBy(dbSwitch)
                    .WriteTo.MSSqlServer(                        
                        connectionString: options.Database.ConnectionString,
                        sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
                        {
                            TableName = "Logs",
                            AutoCreateSqlTable = true
                        }));
            }

            return config;
        }

        private static LogEventLevel ParseLogLevel(string level) =>
            Enum.TryParse<LogEventLevel>(level, ignoreCase: true, out var result)
                ? result
                : LogEventLevel.Information;
    }
}
