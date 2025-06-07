using EventBankingCo.Core.DataAccess.Implementation;
using EventBankingCo.Core.Logging.Implementation;
using EventBankingCo.Core.RequestHandling.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventBankingCo.Core.ApiShared.Startup
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiShared<T>(this IServiceCollection services, IConfiguration config, ApiSharedConfigs sharedConfigs)
        {
            services.AddLogging(config, sharedConfigs.ServiceName, sharedConfigs.LoggingSinkOptions);

            services.AddRequestHandling(HandlerDictionary.FromAssemblyOf<T>());

            services.AddSqlServerDataAccess(sharedConfigs.DatabaseConnectionString);

            return services;
        }
    }
}
