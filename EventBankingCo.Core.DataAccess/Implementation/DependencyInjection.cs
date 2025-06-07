using EventBankingCo.Core.DataAccess.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace EventBankingCo.Core.DataAccess.Implementation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSqlServerDataAccess(this IServiceCollection services, string connectionString)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
            }

            services.AddSingleton<IDbConnectionFactory>(new SqlServerConnectionFactory(connectionString));

            services.AddSingleton<IDataAccess, DataAccess>();

            return services;
        }
    }
}
