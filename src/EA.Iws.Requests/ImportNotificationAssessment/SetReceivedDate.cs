namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportNotificationAssessment)]
    public class SetReceivedDate : IRequest<bool>
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime ReceivedDate { get; private set; }

        public SetReceivedDate(Guid importNotificationId, DateTime receivedDate)
        {
            ImportNotificationId = importNotificationId;
            ReceivedDate = receivedDate;
        }
    }
}
