namespace EA.Iws.Api.Client.Actions
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Entities;
    using Extensions;

    internal class Producer : IProducer
    {
        private readonly HttpClient client;

        public Producer(HttpClient httpClient)
        {
            client = httpClient;
        }

        public async Task<Response<string>> CreateProducer(string accessToken, ProducerData producerData)
        {
            var response = await client.PostAsJsonAsync(accessToken, "Producer/Create", producerData);
            return await response.CreateResponseAsync<string>();
        }
    }
}
