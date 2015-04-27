namespace EA.Iws.Api.IdSrv
{
    using Thinktecture.IdentityServer.Core.Logging;

    internal class DebugLogger : ILogProvider
    {
        public ILog GetLogger(string name)
        {
            return new DebugLog();
        }
    }
}