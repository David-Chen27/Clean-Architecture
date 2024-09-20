using System.Text;
using Murmur;
using Serilog.Core;
using Serilog.Events;

namespace Clean_Architecture.Infrastructure.Logging;

public class EventTypeEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        //紀錄同一件事件的類型Hash Code
        var hash = MurmurHash.Create32().ComputeHash(Encoding.UTF8.GetBytes(logEvent.MessageTemplate.Text));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("EventType", BitConverter.ToUInt32(hash, 0)));
    }
}
