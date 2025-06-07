using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace EventBankingCo.Core.Logging.Enrichers
{
    public class TraceIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var traceId = Activity.Current?.TraceId.ToString() ?? "N/A";

            var traceProperty = propertyFactory.CreateProperty("TraceId", traceId);

            logEvent.AddPropertyIfAbsent(traceProperty);
        }
    }
}
