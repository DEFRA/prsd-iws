namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using Core.Notification;
    using Core.Shared;
    using Core.Shipment;
    using Requests.Notification;

    public class AmountsAndDatesViewModel
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool IsIntendedShipmentsCompleted { get; set; }
        public ShipmentData ShipmentData { get; set; }

        public AmountsAndDatesViewModel()
        {
        }

        public AmountsAndDatesViewModel(AmountsAndDatesInfo amountAndDatesInfo)
        {
            NotificationId = amountAndDatesInfo.NotificationId;
            NotificationType = amountAndDatesInfo.NotificationType;
            IsIntendedShipmentsCompleted = amountAndDatesInfo.IsIntendedShipmentsCompleted;
            ShipmentData = amountAndDatesInfo.ShipmentData ?? new ShipmentData();
        }
    }
}