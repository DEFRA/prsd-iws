namespace EA.Iws.Api.Client.Extensions
{
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;

    internal static class HttpClientExtensions
    {
        public static async Task<T> GetAsync<T>(this HttpClient client, string requestUri)
        {
            var result = await client.GetAsync(requestUri);
            return await result.Content.ReadAsAsync<T>();
        }

        public static async Task<T> GetProtectedAsync<T>(this HttpClient client, string accessToken, string requestUri)
        {
            client.SetBearerToken(accessToken);
            return await client.GetAsync<T>(requestUri);
        }

        public static async Task<HttpResponseMessage> PostProtectedAsync<T>(this HttpClient client, string accessToken, string requestUri, T data)
        {
            client.SetBearerToken(accessToken);
            var result = await client.PostAsync<T>(requestUri, data, new JsonMediaTypeFormatter());
            return result;
        }
    }
}