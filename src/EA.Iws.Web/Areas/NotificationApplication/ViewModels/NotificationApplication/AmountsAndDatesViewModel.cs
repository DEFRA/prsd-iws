namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using Core.IntendedShipments;
    using Core.Shared;
    using Requests.Notification.Overview;

    public class AmountsAndDatesViewModel
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool IsIntendedShipmentsCompleted { get; set; }
        public IntendedShipmentData IntendedShipmentData { get; set; }

        public AmountsAndDatesViewModel()
        {
        }

        public AmountsAndDatesViewModel(AmountsAndDates amountAndDatesInfo)
        {
            NotificationId = amountAndDatesInfo.NotificationId;
            NotificationType = amountAndDatesInfo.NotificationType;
            IsIntendedShipmentsCompleted = amountAndDatesInfo.IsIntendedShipmentsCompleted;
            IntendedShipmentData = amountAndDatesInfo.IntendedShipmentData ?? new IntendedShipmentData();
        }
    }
}