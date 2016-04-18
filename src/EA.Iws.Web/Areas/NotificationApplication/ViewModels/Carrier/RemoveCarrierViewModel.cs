namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Carrier
{
    using System;

    public class RemoveCarrierViewModel
    {
        public Guid NotificationId { get; set; }

        public Guid CarrierId { get; set; }

        public string CarrierName { get; set; }
    }
}