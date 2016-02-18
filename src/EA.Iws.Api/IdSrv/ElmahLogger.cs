namespace EA.Iws.Api.IdSrv
{
    using Elmah;
    using Serilog.Core;
    using Serilog.Events;

    internal class ElmahLogger : ILogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
            ErrorSignal.FromCurrentContext().Raise(logEvent.Exception);
        }
    }
}