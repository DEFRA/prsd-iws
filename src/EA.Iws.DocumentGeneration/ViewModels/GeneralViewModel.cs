namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.Notification;

    internal class GeneralViewModel
    {
        public GeneralViewModel(NotificationApplication notification)
        {
            Number = notification.NotificationNumber;
            IsDisposal = notification.NotificationType.Equals(NotificationType.Disposal);
            IsRecovery = notification.NotificationType.Equals(NotificationType.Recovery);

            var isPreconsented = notification.IsPreconsentedRecoveryFacility;
            if (!isPreconsented.HasValue)
            {
                IsPreconsented = false;
                IsNotPreconsented = true;
            }
            else
            {
                IsPreconsented = isPreconsented.GetValueOrDefault();
                IsNotPreconsented = !isPreconsented.GetValueOrDefault();
            }

            if (notification.ShipmentInfo.NumberOfShipments > 1)
            {
                IsIndividualShipment = false;
                IsNotIndividualShipment = true;
            }
            else
            {
                IsIndividualShipment = true;
                IsNotIndividualShipment = false;
            }
        }

        public string Number { get; private set; }

        public bool IsDisposal { get; private set; }

        public bool IsRecovery { get; private set; }

        public bool IsPreconsented { get; private set; }

        public bool IsNotPreconsented { get; private set; }

        public bool IsIndividualShipment { get; private set; }

        public bool IsNotIndividualShipment { get; private set; }
    }
}