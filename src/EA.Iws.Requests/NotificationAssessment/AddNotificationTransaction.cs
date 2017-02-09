namespace EA.Iws.Requests.NotificationAssessment
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class AddNotificationTransaction : IRequest<bool>
    {
        public AddNotificationTransaction(NotificationTransactionData data)
        {
            Data = data;
        }

        public NotificationTransactionData Data { get; private set; }
    }
}
