namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System.Linq;
    using Core.PackagingType;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Formatters;

    internal class GeneralViewModel
    {
        public GeneralViewModel(NotificationApplication notification,
            ShipmentInfo shipmentInfo,
            FacilityCollection facilityCollection,
            DateTimeFormatter dateTimeFormatter,
            QuantityFormatter quantityFormatter,
            PhysicalCharacteristicsFormatter physicalCharacteristicsFormatter)
        {
            Number = notification.NotificationNumber;
            IsDisposal = notification.NotificationType.Equals(NotificationType.Disposal);
            IsRecovery = notification.NotificationType.Equals(NotificationType.Recovery);

            var isPreconsented = facilityCollection.AllFacilitiesPreconsented;
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

            if (shipmentInfo.NumberOfShipments > 1)
            {
                IsIndividualShipment = false;
                IsNotIndividualShipment = true;
            }
            else
            {
                IsIndividualShipment = true;
                IsNotIndividualShipment = false;
            }

            IntendedNumberOfShipments = shipmentInfo.NumberOfShipments.ToString();
            FirstDeparture = dateTimeFormatter.DateTimeToDocumentFormatString(shipmentInfo.ShipmentPeriod.FirstDate);
            LastDeparture = dateTimeFormatter.DateTimeToDocumentFormatString(shipmentInfo.ShipmentPeriod.LastDate);
            SetIntendedQuantityFields(shipmentInfo, quantityFormatter);

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

            PackagingTypes = GetPackagingInfo(notification);

            PhysicalCharacteristics =
                physicalCharacteristicsFormatter.PhysicalCharacteristicsToCommaDelimitedString(
                    notification.PhysicalCharacteristics);
        }

        private static string GetPackagingInfo(NotificationApplication notification)
        {
            var pistring = string.Empty;
            var packagingTypeList = notification.PackagingInfos.OrderBy(c => c.PackagingType).ToList();

            for (int i = 0; i < packagingTypeList.Count(); i++)
            {
                pistring = pistring + (packagingTypeList[i].PackagingType != PackagingType.Other
                    ? packagingTypeList[i].PackagingType.ToString()
                    : packagingTypeList[i].PackagingType + "(" + packagingTypeList[i].OtherDescription + ")");

                if (i < (packagingTypeList.Count() - 1))
                {
                    pistring = pistring + ", ";
                }
            }

            return pistring;
        }

        private void SetIntendedQuantityFields(ShipmentInfo shipmentInfo, QuantityFormatter quantityFormatter)
        {
            IntendedQuantityTonnes = string.Empty;
            IntendedQuantityKg = string.Empty;
            IntendedQuantityM3 = string.Empty;
            IntendedQuantityLtrs = string.Empty;

            var quantity = quantityFormatter.QuantityToStringWithUnits(shipmentInfo.Quantity,
                        shipmentInfo.Units);

            switch (shipmentInfo.Units)
            {
                case ShipmentQuantityUnits.Kilograms:
                    IntendedQuantityKg = quantity;
                    break;
                case ShipmentQuantityUnits.CubicMetres:
                    IntendedQuantityM3 = quantity;
                    break;
                case ShipmentQuantityUnits.Litres:
                    IntendedQuantityLtrs = quantity;
                    break;
                case ShipmentQuantityUnits.Tonnes:
                    IntendedQuantityTonnes = quantity;
                    break;
                default:
                    break;
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

        public string IntendedQuantityTonnes { get; private set; }

        public string IntendedQuantityKg { get; private set; }

        public string IntendedQuantityM3 { get; private set; }

        public string IntendedQuantityLtrs { get; private set; }

        public string PhysicalCharacteristics { get; private set; }
    }
}