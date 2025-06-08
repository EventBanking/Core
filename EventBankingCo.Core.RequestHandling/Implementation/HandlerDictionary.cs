using EventBankingCo.Core.RequestHandling.Abstraction;
using EventBankingCo.Core.RequestHandling.Base;

namespace EventBankingCo.Core.RequestHandling.Implementation
{
    public class HandlerDictionary : Dictionary<Type, Type>, IHandlerDictionary
    {
        private HandlerDictionary() { }

        public static IHandlerDictionary FromAssemblyOf<T>()
        {
            var dictionary = new HandlerDictionary();

            var handlerType = typeof(IRequestHandler<>);
            var handlerDefinition = handlerType.GetGenericTypeDefinition();

            foreach (var type in typeof(T).Assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    if (ImplementsGenericTypeDefinition(type, handlerDefinition))
                    {
                        var requestType = type.GetInterface(handlerType.Name)!.GenericTypeArguments.Single();
                    
                        dictionary.Add(requestType, type);
                    }
                }
            }

            return dictionary;
        }

        #region Public IHandlerDictionary Method

        public Type GetHandlerType(Type requestType)
        {
            if (requestType is null)
            {
                throw new ArgumentNullException(nameof(requestType), "Request type cannot be null.");
            }

            if (!TryGetValue(requestType, out var handlerType))
            {
                throw new KeyNotFoundException($"No handler found for request type {requestType.Name}.");
            }

            return handlerType;
        }

        public Dictionary<Type, Type> GetDictionary() => this;

        #endregion

        private static bool ImplementsGenericTypeDefinition(Type? type, Type genericTypeDefinition) =>
            type?.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeDefinition) ?? false;
    }
}
