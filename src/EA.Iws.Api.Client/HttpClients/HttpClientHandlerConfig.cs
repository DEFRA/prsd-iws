namespace EA.Iws.Api.Client.HttpClients
{
    public class HttpClientHandlerConfig
    {
        public bool ProxyEnabled { get; set; }

        public bool ByPassProxyOnLocal { get; set; }

        public string ProxyWebAddress { get; set; }

        public bool ProxyUseDefaultCredentials { get; set; }
    }
}
