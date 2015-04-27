namespace EA.Iws.Api.IdSrv
{
    using System.Diagnostics;
    using Thinktecture.IdentityServer.Core.Logging;

    internal class DebugLog : ILog
    {
        public bool Log(LogLevel logLevel, System.Func<string> messageFunc, System.Exception exception = null)
        {
            if (messageFunc != null)
            {
                Debug.WriteLine(messageFunc());
            }
            if (exception != null)
            {
                Debug.WriteLine(exception.ToString());
            }
            return true;
        }
    }
}