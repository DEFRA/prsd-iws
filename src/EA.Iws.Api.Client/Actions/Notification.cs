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
            return
                await
                    client.GetAsync<NotificationInformation>(accessToken,
                        string.Format("NotificationInformation/GetById/{0}", id));
        }

        public async Task<Response<byte[]>> GenerateNotificationDocumentAsync(string accessToken, Guid id)
        {
            client.SetBearerToken(accessToken);
            var response = await client.GetAsync(string.Format("DocumentGeneration/{0}", id));

            return await response.CreateResponseByteArrayAsync();
        }

        public async Task<Response<NotificationApplicationSummaryData[]>> GetUserNotifications(string accessToken)
        {
            client.SetBearerToken(accessToken);

            // ConfigureAwait(false) must be used otherwise the calling non-async action will deadlock.
            var response = await client.GetAsync(string.Format("NotificationInformation/GetUserNotifications")).ConfigureAwait(false);

            // Do not remove ConfigureAwait.
            return await response.CreateResponseAsync<NotificationApplicationSummaryData[]>().ConfigureAwait(false);
        }
    }
}