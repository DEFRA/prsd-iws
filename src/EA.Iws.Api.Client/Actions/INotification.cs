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
    }
}