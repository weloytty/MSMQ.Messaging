using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;

namespace MSMQ
{
  internal sealed class TelemetryEventSource : EventSource
  {
    private static readonly string[] telemetryTraits = new string[2]
    {
      "ETW_GROUP",
      "{4f50731a-89cf-4782-b3e0-dce8c90476ba}"
    };
    internal const EventKeywords Reserved44Keyword = (EventKeywords) 17592186044416;
    internal const EventKeywords TelemetryKeyword = (EventKeywords) 35184372088832;
    internal const EventKeywords MeasuresKeyword = (EventKeywords) 70368744177664;
    internal const EventKeywords CriticalDataKeyword = (EventKeywords) 140737488355328;
    internal const EventTags CoreData = (EventTags) 524288;
    internal const EventTags InjectXToken = (EventTags) 1048576;
    internal const EventTags RealtimeLatency = (EventTags) 2097152;
    internal const EventTags NormalLatency = (EventTags) 4194304;
    internal const EventTags CriticalPersistence = (EventTags) 8388608;
    internal const EventTags NormalPersistence = (EventTags) 16777216;
    internal const EventTags DropPii = (EventTags) 33554432;
    internal const EventTags HashPii = (EventTags) 67108864;
    internal const EventTags MarkPii = (EventTags) 134217728;
    internal const EventFieldTags DropPiiField = (EventFieldTags) 67108864;
    internal const EventFieldTags HashPiiField = (EventFieldTags) 134217728;
    private const string MessagingProviderName = "Microsoft.DOTNET.System.Messaging";

    internal TelemetryEventSource(string eventSourceName):base(eventSourceName,(EventSourceSettings) 8,TelemetryEventSource.telemetryTraits)
    {
    
    }

    internal static EventSourceOptions TelemetryOptions()
    {
        EventSourceOptions eventSourceOptions = new EventSourceOptions {Keywords = ((EventKeywords) 35184372088832L)};
        return eventSourceOptions;
    }

    internal static EventSourceOptions MeasuresOptions()
    {
        EventSourceOptions eventSourceOptions = new EventSourceOptions {Keywords = ((EventKeywords) 70368744177664L)};
        return eventSourceOptions;
    }

    internal static EventSourceOptions CriticalDataOptions()
    {
            EventSourceOptions eventSourceOptions = new EventSourceOptions
            {
                Keywords = ((EventKeywords)140737488355328L)
            };
            return eventSourceOptions;
    }

    private void WriteUsageEvent([CallerMemberName] string memberName = "")
    {
      this.Write(memberName, TelemetryEventSource.MeasuresOptions());
    }

    internal TelemetryEventSource()
      : this("Microsoft.DOTNET.System.Messaging")
    {
    }

    internal void MessageQueue()
    {
      this.WriteUsageEvent(nameof (MessageQueue));
    }
  }
}
