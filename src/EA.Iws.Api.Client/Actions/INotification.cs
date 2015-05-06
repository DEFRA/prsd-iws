namespace EA.Iws.Api.Client.Actions
{
    using System;
    using System.Threading.Tasks;
    using Entities;

    public interface INotification
    {
        Task<Response<Guid>> CreateNotificationApplicationAsync(string accessToken, CreateNotificationData notificationData);

        Task<NotificationInformation> GetNotificationInformationAsync(string accessToken, Guid id);

        Task<Response<byte[]>> GenerateNotificationDocumentAsync(string accessToken, Guid id);

        /// <summary>
        /// Gets summary data for the notifications a user has generated.
        /// </summary>
        /// <param name="accessToken">The user access token stored in the authentication cookie.</param>
        /// <returns>An array of the summary data for notifications this user has generated.</returns>
        Task<Response<NotificationApplicationSummaryData[]>> GetUserNotifications(string accessToken);
    }
}