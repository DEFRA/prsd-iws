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
        public NotificationApplicationCompletionProgress Progress { get; set; }
        public ShipmentData ShipmentData { get; set; }

        public AmountsAndDatesViewModel()
        {
        }

        public AmountsAndDatesViewModel(AmountsAndDatesInfo amountAndDatesInfo)
        {
            NotificationId = amountAndDatesInfo.NotificationId;
            NotificationType = amountAndDatesInfo.NotificationType;
            Progress = amountAndDatesInfo.Progress;
            ShipmentData = amountAndDatesInfo.ShipmentData ?? new ShipmentData();
        }
    }
}