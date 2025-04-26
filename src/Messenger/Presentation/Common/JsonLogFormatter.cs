using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;

namespace Messenger.Presentation.Common;

public class JsonLogFormatter : ITextFormatter
{
    private readonly JsonValueFormatter _valueFormatter = new JsonValueFormatter();

    public void Format(LogEvent logEvent, TextWriter output)
    {
        output.Write("{\"Timestamp\":\"");
        output.Write(logEvent.Timestamp.UtcDateTime.ToString("O"));

        output.Write("\",\"Level\":\"");
        output.Write(logEvent.Level.ToString().ToLower());

        output.Write("\",\"Message\":");
        JsonValueFormatter.WriteQuotedJsonString(logEvent.RenderMessage(), output);

        if (logEvent.Exception != null)
        {
            output.Write(",\"Exception\":");
            JsonValueFormatter.WriteQuotedJsonString(logEvent.Exception.ToString(), output);
        }

        if (logEvent.TraceId != null)
        {
            output.Write(",\"TraceId\":\"");
            output.Write(logEvent.TraceId.Value.ToHexString());
            output.Write('\"');
        }

        if (logEvent.SpanId != null)
        {
            output.Write(",\"SpanId\":\"");
            output.Write(logEvent.SpanId.Value.ToHexString());
            output.Write('\"');
        }

        foreach (var property in logEvent.Properties)
        {
            output.Write(',');
            JsonValueFormatter.WriteQuotedJsonString(property.Key, output);
            output.Write(':');
            _valueFormatter.Format(property.Value, output);
        }

        output.Write("}");

        output.WriteLine();
    }
}
