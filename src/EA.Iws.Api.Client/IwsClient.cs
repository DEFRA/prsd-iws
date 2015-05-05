namespace EA.Iws.Api.Client
{
    using System;
    using System.Net.Http;
    using Actions;

    public class IwsClient : IDisposable, IIwsClient
    {
        private readonly HttpClient httpClient;
        private IRegistration registration;
        private INotification notification;

        public IwsClient(string baseUrl)
        {
            var baseUri = new Uri(baseUrl);

            httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUri, "/api/")
            };
        }

        public IRegistration Registration
        {
            get { return registration ?? (registration = new Registration(httpClient)); }
        }

        public INotification Notification
        {
            get { return notification ?? (notification = new Notification(httpClient)); }
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}