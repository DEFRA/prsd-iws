namespace EA.Iws.Scanning
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Api.Client;
    using Api.Client.Actions;
    using Prsd.Core.Web.Converters;
    using Prsd.Core.Web.Extensions;

    public class IwsScanClient : IIwsScanClient
    {
        private readonly HttpClient httpClient;
        private IErrorLog errorLog;

        public IwsScanClient(string baseUrl)
        {
            var baseUri = new Uri(baseUrl.EnsureTrailingSlash());

            httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUri, "api/")
            };
        }

        public IErrorLog ErrorLog
        {
            get { return errorLog ?? (errorLog = new ErrorLog(httpClient)); }
        }

        public async Task<ScanResult> ScanAsync(string accessToken, byte[] file)
        {
            httpClient.SetBearerToken(accessToken);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/bson"));

            //var formatter = new BsonMediaTypeFormatter();

            var response = await httpClient.PostAsJsonAsync("scanner/Scan", file).ConfigureAwait(false);

            return await response.CreateResponseAsync<ScanResult>(new EnumerationConverter()).ConfigureAwait(false);
        }
    }
}