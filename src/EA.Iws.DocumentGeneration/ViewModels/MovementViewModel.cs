namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Core.Shared;
    using Domain;
    using Domain.Movement;
    using Formatters;

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

        public bool IsSpecialHandling { get; set; }
        public bool IsNotSpecialHandling { get; set; }

        public MovementViewModel(Movement movement, 
            DateTimeFormatter dateTimeFormatter, 
            QuantityFormatter quantityFormatter,
            PhysicalCharacteristicsFormatter physicalCharacteristicsFormatter)
        {
            if (movement == null)
            {
                return;
            }

            ActualDate = dateTimeFormatter.DateTimeToDocumentFormatString(movement.Date);
            SetQuantity(movement, quantityFormatter);
            Number = movement.Number.ToString();

            if (movement.NotificationApplication != null)
            {
                NotificationNumber = movement.NotificationApplication.NotificationNumber ?? string.Empty;
                IsSpecialHandling = movement.NotificationApplication.HasSpecialHandlingRequirements.GetValueOrDefault();
                IsNotSpecialHandling = !movement.NotificationApplication.HasSpecialHandlingRequirements.GetValueOrDefault(true);
                PhysicalCharacteristics =
                    physicalCharacteristicsFormatter.PhysicalCharacteristicsToCommaDelimitedString(
                        movement.NotificationApplication.PhysicalCharacteristics);
            }
        }

        private void SetQuantity(Movement movement, QuantityFormatter quantityFormatter)
        {
            if (!movement.DisplayUnits.HasValue || !movement.Units.HasValue || !movement.Quantity.HasValue)
            {
                return;
            }

            var value = ShipmentQuantityUnitConverter.ConvertToTarget(movement.Units.Value, movement.DisplayUnits.Value,
                movement.Quantity.Value);
            var displayString = quantityFormatter.QuantityToStringWithUnits(value, movement.DisplayUnits.Value);

            switch (movement.DisplayUnits.Value)
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