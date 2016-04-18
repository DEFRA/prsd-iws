namespace EA.Iws.Core.Movement
{
    using System.Collections.Generic;
    using PackagingType;
    using Shared;

    public class NewMovementDetails
    {
        public decimal Quantity { get; set; }

        public ShipmentQuantityUnits Units { get; set; }

        public IList<PackagingType> PackagingTypes { get; set; }
    }
}