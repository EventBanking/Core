using EventBankingCo.Core.RequestHandling.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace EventBankingCo.Core.RequestHandling.Implementation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRequestHandling(this IServiceCollection services, IHandlerDictionary handlerDictionary)
        {
            services.AddSingleton(handlerDictionary);

            services.AddSingleton<IHandlerFactory, HandlerFactory>();

            services.AddSingleton<IRequestProcessor, RequestProcessor>();

            services.AddSingleton<ITypeInstantiator, TypeInstantiator>();

            return services;
        }
    }
}
