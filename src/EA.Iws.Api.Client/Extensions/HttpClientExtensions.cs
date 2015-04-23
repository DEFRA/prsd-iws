namespace EA.Iws.Api.Client.Extensions
{
    using System.Net.Http;
    using System.Threading.Tasks;

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
    }
}