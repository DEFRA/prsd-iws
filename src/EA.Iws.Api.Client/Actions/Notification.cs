namespace EA.Iws.Api.Client.Actions
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Entities;
    using Extensions;

    internal class Notification : INotification
    {
        private readonly HttpClient client;

        public Notification(HttpClient httpClient)
        {
            client = httpClient;
        }

        public async Task<Response<Guid>> CreateNotificationApplicationAsync(string accessToken,
            CreateNotificationData notificationData)
        {
            var response = await client.PostAsJsonAsync(accessToken, "NotificationApplication", notificationData);
            return await response.CreateResponseAsync<Guid>();
        }

        public async Task<NotificationInformation> GetNotificationInformationAsync(string accessToken, Guid id)
        {
            return await client.GetAsync<NotificationInformation>(accessToken, string.Format("NotificationInformation/{0}", id));
        }
    }
}