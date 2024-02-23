namespace EA.Iws.Requests.AdditionalCharge
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.ImportNotification.AdditionalCharge;
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotification)]
    public class CreateImportNotificationAdditionalCharge : IRequest<bool>
    {
        public Guid NotificationId { get; set; }

        public DateTime ChargeDate { get; set; }

        public decimal ChargeAmount { get; set; }

        public AdditionalChargeType ChangeDetailType { get; set; }

        public string Comments { get; set; }
    }
}
