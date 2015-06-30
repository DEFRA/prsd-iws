namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System.Linq;
    using System.Security.Policy;
    using DocumentFormat.OpenXml.Office.CustomUI;
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
                IsNotPreconsented = false;
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

            IntendedNumberOfShipments = notification.ShipmentInfo.NumberOfShipments.ToString();
            FirstDeparture = notification.ShipmentInfo.FirstDate.ToShortDateString();
            LastDeparture = notification.ShipmentInfo.LastDate.ToShortDateString();

            var hasSpecialHandlingRequirements = notification.HasSpecialHandlingRequirements;
            if (!hasSpecialHandlingRequirements.HasValue)
            {
                IsSpecialHandling = false;
                IsNotSpecialHandling = false;
            }
            else
            {
                IsSpecialHandling = hasSpecialHandlingRequirements.GetValueOrDefault();
                IsNotSpecialHandling = !hasSpecialHandlingRequirements.GetValueOrDefault();
            }

            var packagingInfo = notification.PackagingInfos;
            foreach (var item in packagingInfo)
            {
                PackagingTypes = PackagingTypes + "  " + item.PackagingType.Value;
            }
        }

        public string Number { get; private set; }

        public bool IsDisposal { get; private set; }

        public bool IsRecovery { get; private set; }

        public bool IsPreconsented { get; private set; }

        public bool IsNotPreconsented { get; private set; }

        public bool IsIndividualShipment { get; private set; }

        public bool IsNotIndividualShipment { get; private set; }

        public string IntendedNumberOfShipments { get; private set; }

        public string FirstDeparture { get; private set; }

        public string LastDeparture { get; private set; }

        public bool IsSpecialHandling { get; private set; }

        public bool IsNotSpecialHandling { get; private set; }

        public string PackagingTypes { get; private set; }
    }
}