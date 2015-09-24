namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Core.Shared;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Formatters;
    using NotificationType = Domain.NotificationApplication.NotificationType;

    public class MovementViewModel
    {
        private string notificationNumber = string.Empty;
        private string number = string.Empty;
        private string actualTonnes = string.Empty;
        private string actualKilograms = string.Empty;
        private string actualLitres = string.Empty;
        private string actualCubicMetres = string.Empty;
        private string actualDate = string.Empty;
        private string physicalCharacteristics = string.Empty;
        private string intendedNumberOfShipments = string.Empty;
        private string numberOfPackages = string.Empty;
        private string packagingTypes = string.Empty;

        public string NotificationNumber
        {
            get { return notificationNumber; }
            set { notificationNumber = value; }
        }
        public string Number
        {
            get { return number; }
            set { number = value; }
        }
        public string ActualTonnes
        {
            get { return actualTonnes; }
            set { actualTonnes = value; }
        }
        public string ActualKilograms
        {
            get { return actualKilograms; }
            set { actualKilograms = value; }
        }
        public string ActualLitres
        {
            get { return actualLitres; }
            set { actualLitres = value; }
        }
        public string ActualCubicMetres
        {
            get { return actualCubicMetres; }
            set { actualCubicMetres = value; }
        }
        public string ActualDate
        {
            get { return actualDate; }
            set { actualDate = value; }
        }
        public string PhysicalCharacteristics
        {
            get { return physicalCharacteristics; }
            set { physicalCharacteristics = value; }
        }
        public string PackagingTypes
        {
            get { return packagingTypes; }
            set { packagingTypes = value; }
        }

        public string IntendedNumberOfShipments
        {
            get { return intendedNumberOfShipments; }
            set { intendedNumberOfShipments = value; }
        }
        public string NumberOfPackages
        {
            get { return numberOfPackages; }
            set { numberOfPackages = value; }
        }

        public bool IsSpecialHandling { get; set; }
        public bool IsNotSpecialHandling { get; set; }
        public bool IsDisposal { get; set; }
        public bool IsRecovery { get; set; }

        public MovementViewModel(Movement movement,
            NotificationApplication notification,
            ShipmentInfo shipmentInfo,
            DateTimeFormatter dateTimeFormatter,
            QuantityFormatter quantityFormatter,
            PhysicalCharacteristicsFormatter physicalCharacteristicsFormatter,
            PackagingTypesFormatter packagingTypesFormatter)
        {
            if (movement == null)
            {
                return;
            }

            ActualDate = dateTimeFormatter.DateTimeToDocumentFormatString(movement.Date);
            SetQuantity(movement, quantityFormatter);
            Number = movement.Number.ToString();
            PackagingTypes = packagingTypesFormatter.PackagingTypesToCommaDelimitedString(movement.PackagingInfos);
            NumberOfPackages = movement.NumberOfPackages.ToString();

            if (notification == null)
            {
                return;
            }

            NotificationNumber = notification.NotificationNumber ?? string.Empty;
            IsSpecialHandling = notification.HasSpecialHandlingRequirements.GetValueOrDefault();
            IsNotSpecialHandling = !notification.HasSpecialHandlingRequirements.GetValueOrDefault(true);
            PhysicalCharacteristics =
                physicalCharacteristicsFormatter.PhysicalCharacteristicsToCommaDelimitedString(notification.PhysicalCharacteristics);
            IntendedNumberOfShipments = (shipmentInfo == null)
                ? "0"
                : shipmentInfo.NumberOfShipments.ToString();
            IsRecovery = notification.NotificationType == NotificationType.Recovery;
            IsDisposal = notification.NotificationType == NotificationType.Disposal;
        }

        private void SetQuantity(Movement movement, QuantityFormatter quantityFormatter)
        {
            if (!movement.Units.HasValue || !movement.Quantity.HasValue)
            {
                return;
            }

            var displayString = quantityFormatter.QuantityToStringWithUnits(movement.Quantity, movement.Units.Value);

            switch (movement.Units.Value)
            {
                case ShipmentQuantityUnits.Kilograms:
                    ActualKilograms = displayString;
                    break;
                case ShipmentQuantityUnits.CubicMetres:
                    ActualCubicMetres = displayString;
                    break;
                case ShipmentQuantityUnits.Litres:
                    ActualLitres = displayString;
                    break;
                case ShipmentQuantityUnits.Tonnes:
                    ActualTonnes = displayString;
                    break;
                default:
                    break;
            }
        }
    }
}