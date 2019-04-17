namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.ShipmentAudit
{
    using System;
    using EA.Iws.Core.Shared;
    using Web.ViewModels.Shared;

    public class ShipmentAuditViewModel
    {
        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public ShipmentAuditTrailViewModel ShipmentAuditModel { get; set; }

        public ShipmentAuditViewModel()
        {
            ShipmentAuditModel = new ShipmentAuditTrailViewModel();
        }

        public ShipmentAuditViewModel(ShipmentAuditData data)
        {
            ShipmentAuditModel = new ShipmentAuditTrailViewModel(data);
        }
    }
}