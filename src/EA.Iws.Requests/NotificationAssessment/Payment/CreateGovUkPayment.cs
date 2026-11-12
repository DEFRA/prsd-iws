namespace EA.Iws.Requests.NotificationAssessment.Payment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class CreateGovUkPayment : IRequest<CreateGovUkPaymentResult>
    {
        public Guid NotificationId { get; private set; }

        public CreateGovUkPayment(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}