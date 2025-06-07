using EventBankingCo.Core.Domain.Constants;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace EventBankingCo.Core.Logging.Enrichers
{
    public class CorrelationIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var correlationId = Guid.Empty.ToString();

            if (Activity.Current != null)
            {
                var context = Activity.Current.Baggage.FirstOrDefault(kvp => kvp.Key == Headers.CorrelationId);

                if (!string.IsNullOrEmpty(context.Value))
                {
                    correlationId = context.Value;
                }
            }

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CorrelationId", correlationId));
        }
    }
}
