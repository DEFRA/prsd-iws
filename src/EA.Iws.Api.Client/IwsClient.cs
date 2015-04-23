namespace EA.Iws.Api.Client
{
    using System;
    using System.Net.Http;

    public class IwsClient : IDisposable
    {
        private readonly HttpClient httpClient;

        public IwsClient(string baseUrl)
        {
            var baseUri = new Uri(baseUrl);

            httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUri, "/api/")
            };
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}