namespace EA.Iws.Api.IdSrv
{
    using System.Diagnostics;
    using System.IO;
    using Serilog.Core;
    using Serilog.Events;
    using Serilog.Formatting.Display;

    internal class DebugLogger : ILogEventSink
    {
        private readonly MessageTemplateTextFormatter formatter;

        public DebugLogger(MessageTemplateTextFormatter formatter)
        {
            this.formatter = formatter;
        }

        public void Emit(LogEvent logEvent)
        {
            var sr = new StringWriter();
            formatter.Format(logEvent, sr);
            var text = sr.ToString().Trim();

            Debug.WriteLine(text);
        }
    }
}