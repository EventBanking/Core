using Serilog.Core;
using Serilog.Events;

namespace EventBankingCo.Core.Logging.Enrichers
{
    public class ClassNameFromSourceContextEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent.Properties.ContainsKey("ClassName"))
            {
                return;
            }

            if (logEvent.Properties.TryGetValue("SourceContext", out var sourceContextValue) &&
                sourceContextValue is ScalarValue { Value: string fullTypeName })
            {
                var className = fullTypeName?.Split('.').LastOrDefault();
                if (!string.IsNullOrWhiteSpace(className))
                {
                    var classNameProperty = propertyFactory.CreateProperty("ClassName", className);
                    logEvent.AddPropertyIfAbsent(classNameProperty);
                }
            }
        }
    }
}
