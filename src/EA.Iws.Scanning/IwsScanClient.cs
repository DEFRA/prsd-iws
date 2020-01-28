namespace EA.Iws.Scanning
{
    using System;
    using System.Net.Http;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using Prsd.Core.Web.Converters;
    using Prsd.Core.Web.Extensions;

    public class IwsScanClient : IIwsScanClient
    {
        private readonly string certPath;
        private readonly Uri baseUri;

        public IwsScanClient(string baseUrl, string certPath)
        {
            this.certPath = certPath;
            this.baseUri = new Uri(baseUrl.EnsureTrailingSlash());
        }

        public async Task<ScanResult> ScanAsync(string accessToken, byte[] file)
        {
            using (var handler = new WebRequestHandler())
            {
                if (!string.IsNullOrWhiteSpace(certPath))
                {
                    handler.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
                    {
                        if (sslPolicyErrors == SslPolicyErrors.None)
                        {
                            return true;
                        }

                        return X509Certificate.CreateFromCertFile(certPath).Equals(certificate);
                    };
                }

                using (var httpClient = new HttpClient(handler) { BaseAddress = new Uri(baseUri, "api/") })
                {
                    httpClient.SetBearerToken(accessToken);
                    httpClient.DefaultRequestHeaders.Accept.Clear();

                    var response = await httpClient.PostAsJsonAsync("scanner/Scan", file).ConfigureAwait(false);

                    var result = await response.CreateResponseAsync<ScanResult>(new EnumerationConverter())
                        .ConfigureAwait(false);

                    return result;
                };
            }
        }
    }
}