namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class CreateFinancialGuarantee : IRequest<Guid>
    {
        public CreateFinancialGuarantee(Guid notificationId, DateTime receivedDate)
        {
            NotificationId = notificationId;
            ReceivedDate = receivedDate;
        }

        public Guid NotificationId { get; private set; }

        public DateTime ReceivedDate { get; private set; }
    }
}