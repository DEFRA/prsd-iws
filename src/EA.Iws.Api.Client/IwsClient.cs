namespace EA.Iws.Api.Client
{
    using System;
    using System.Net.Http;
    using Actions;

    public class IwsClient : IDisposable
    {
        private readonly HttpClient httpClient;
        private IApplicantRegistration applicantRegistration;

        public IwsClient(string baseUrl)
        {
            var baseUri = new Uri(baseUrl);

            httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUri, "/api/")
            };
        }

        public IApplicantRegistration ApplicantRegistration
        {
            get { return applicantRegistration ?? (applicantRegistration = new ApplicantRegistration(httpClient)); }
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}