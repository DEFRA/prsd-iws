namespace EA.Iws.Core.Movement
{
    using PackagingType;
    using Shared;

    public class ShipmentInfo
    {
        public ShipmentDates ShipmentDates { get; set; }

        public ShipmentQuantityUnits ShipmentQuantityUnits { get; set; }

        public PackagingData PackagingData { get; set; }
    }
}