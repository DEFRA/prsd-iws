namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.ReceiptRecovery
{
    using Core.Shared;
    using System;
    public class SuccessViewModel
    {
        public Guid NotificationId { get; set; }

        public CertificateType Certificate { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}