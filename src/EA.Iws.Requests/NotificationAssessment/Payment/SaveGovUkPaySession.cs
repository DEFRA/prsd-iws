namespace EA.Iws.Requests.NotificationAssessment.Payment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class SaveGovUkPaySession : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public string PaymentId { get; private set; }

        public string PaymentReference { get; private set; }

        public string SecureToken { get; private set; }

        public decimal Amount { get; private set; }

        public SaveGovUkPaySession(Guid notificationId, string paymentId, string paymentReference, string secureToken, decimal amount)
        {
            NotificationId = notificationId;
            PaymentId = paymentId;
            PaymentReference = paymentReference;
            SecureToken = secureToken;
            Amount = amount;
        }
    }
}