using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.Logging.Configurations;
using EventBankingCo.Core.Logging.Implementation;
using EventBankingCo.Core.Logging.Extensions;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics;

namespace EventBankingCo.Core.Logging.DependencyInjection
{
    public static class LoggingInjection
    {
        public static IServiceCollection AddCoreLogging(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection("LoggingOptions").Get<LoggingOptions>() ?? new();

            var loggerConfig = new LoggerConfiguration();
            loggerConfig = loggerConfig.ApplyCoreConfiguration(options, configuration, out var levelSwitches);

            Log.Logger = loggerConfig.CreateLogger();

            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

            services.AddSingleton(levelSwitches); // Optional if runtime adjustment is desired
            
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);
            });

            services.AddSingleton<ICoreLogger, CoreLogger>();
            return services;
        }
    }
}
