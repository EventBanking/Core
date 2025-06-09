using Azure;
using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace EventBankingCo.Core.RequestHandling.Implementation
{
    /// <summary>
    /// This class implements the ITypeActivator in order to instantiate objects at runtime. 
    /// Uses IServiceProvider and ActivatorUtilities to instantiate objects.
    /// </summary>
    public class TypeInstantiator : ITypeInstantiator
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ICoreLogger<TypeInstantiator> _logger;

        public TypeInstantiator(IServiceProvider serviceProvider, ICoreLoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;

            _logger = loggerFactory.Create(this);
        }

        public object Instantiate(Type typeToInstantiate)
        {
            _logger.LogDebug("Attempting to instantiate", typeToInstantiate.Name);

            try
            {
                var obj = ActivatorUtilities.CreateInstance(_serviceProvider, typeToInstantiate);

                if (obj != null)
                {
                    _logger.LogDebug("Instantiated Object", obj.GetType().Name);

                    return obj;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while instantiating the type.", e, typeToInstantiate.Name);

                throw;
            }

            var exception = new InvalidOperationException($"Could not instantiate type {typeToInstantiate.Name}.");

            _logger.LogError(exception.Message, exception, typeToInstantiate.Name);

            throw exception;
        }
    }
}
