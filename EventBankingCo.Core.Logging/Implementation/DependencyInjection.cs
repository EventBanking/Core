using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.Logging.Implementation;
using System.Diagnostics;
using EventBankingCo.Core.Logging.Enrichers;
using Serilog.Exceptions;

namespace EventBankingCo.Core.Logging.Implementation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration config, string serviceName, params ISinkOption[] sinkOptions)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (sinkOptions is null || sinkOptions.Length == 0)
            {
                return services;
            }

            var loggerConfig = new LoggerConfiguration();

            // Apply Enrichers
            loggerConfig.ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ServiceName", serviceName)      
                .Enrich.With(new ClassNameFromSourceContextEnricher())
                .Enrich.With(new CorrelationIdEnricher())
                .Enrich.With(new TraceIdEnricher())
                .Enrich.WithExceptionDetails();

            // Apply Sinks
            foreach (var sinkOption in sinkOptions)
            {
                if (sinkOption.IsEnabled())
                {
                    // Add sinkOption to services to enable updating LogLevelSwitch at runtime
                    services.AddSingleton(sinkOption);

                    loggerConfig.WriteTo.Logger(lc =>
                        sinkOption.ConfigureSink(lc)
                                  .MinimumLevel.ControlledBy(sinkOption.GetLogLevelSwitch())
                    );
                }
            }

            Log.Logger = loggerConfig.CreateLogger();

            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);
            });

            services.AddSingleton<ICoreLoggerFactory, CoreLoggerFactory>();

            return services;
        }
    }
}
