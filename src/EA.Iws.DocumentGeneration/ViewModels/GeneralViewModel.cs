namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System.Linq;
    using Core.Shared;
    using Domain.NotificationApplication;
    using NotificationType = Domain.NotificationApplication.NotificationType;

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
            FirstDeparture = notification.ShipmentInfo.FirstDate.ToString("dd.MM.yy");
            LastDeparture = notification.ShipmentInfo.LastDate.ToString("dd.MM.yy");
            SetIntendedQuantityFields(notification.ShipmentInfo);

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

            PhysicalCharacteristics = GetPhysicalCharacteristics(notification);
        }

        private static string GetPackagingInfo(NotificationApplication notification)
        {
            var pistring = string.Empty;
            var packagingTypeList = notification.PackagingInfos.OrderBy(c => c.PackagingType.Value).ToList();

            for (int i = 0; i < packagingTypeList.Count(); i++)
            {
                pistring = pistring + (packagingTypeList[i].PackagingType != PackagingType.Other
                    ? packagingTypeList[i].PackagingType.Value.ToString()
                    : packagingTypeList[i].PackagingType.Value + "(" + packagingTypeList[i].OtherDescription + ")");

                if (i < (packagingTypeList.Count() - 1))
                {
                    pistring = pistring + ", ";
                }
            }

            return pistring;
        }

        private static string GetPhysicalCharacteristics(NotificationApplication notification)
        {
            var pcstring = string.Empty;
            var pclist = notification.PhysicalCharacteristics.OrderBy(c => c.PhysicalCharacteristic.Value).ToList();

            for (int i = 0; i < notification.PhysicalCharacteristics.Count(); i++)
            {
                pcstring = pcstring + (pclist[i].PhysicalCharacteristic != PhysicalCharacteristicType.Other
                    ? pclist[i].PhysicalCharacteristic.Value.ToString()
                    : pclist[i].PhysicalCharacteristic.Value + "(" + pclist[i].OtherDescription + ")");

                if (i < (notification.PhysicalCharacteristics.Count() - 1))
                {
                    pcstring = pcstring + ", ";
                }
            }

            return pcstring;
        }

        private void SetIntendedQuantityFields(ShipmentInfo shipmentInfo)
        {
            IntendedQuantityTonnes = string.Empty;
            IntendedQuantityKg = string.Empty;
            IntendedQuantityM3 = string.Empty;
            IntendedQuantityLtrs = string.Empty;
            if (shipmentInfo.Units == ShipmentQuantityUnits.CubicMetres)
            {
                IntendedQuantityM3 = shipmentInfo.Quantity.ToString();
            }
            else if (shipmentInfo.Units == ShipmentQuantityUnits.Kilograms)
            {
                IntendedQuantityKg = shipmentInfo.Quantity + " kg";
            }
            else if (shipmentInfo.Units == ShipmentQuantityUnits.Tonnes)
            {
                IntendedQuantityTonnes = shipmentInfo.Quantity.ToString();
            }
            else if (shipmentInfo.Units == ShipmentQuantityUnits.Litres)
            {
                IntendedQuantityLtrs = shipmentInfo.Quantity + " Ltrs";
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