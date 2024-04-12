namespace EA.Iws.Requests.AdditionalCharge
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Iws.Core.Shared;
    using EA.Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class CreateAdditionalCharge : IRequest<bool>
    {
        public CreateAdditionalCharge() 
        {
        }

        public CreateAdditionalCharge(Guid notificationId, AdditionalChargeData model, AdditionalChargeType additionalChargeType)
        {
            ChangeDetailType = additionalChargeType;
            ChargeAmount = model.Amount;
            Comments = model.Comments;
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; set; }

        public DateTime ChargeDate { get; set; }

        public decimal ChargeAmount { get; set; }

        public AdditionalChargeType ChangeDetailType { get; set; }

        public string Comments { get; set; }
    }
}
