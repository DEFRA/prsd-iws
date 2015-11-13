namespace EA.Iws.Core.Movement
{
    using System;
    using System.Collections.Generic;
    using PackagingType;
    using Shared;

    public class NewMovementDetails
    {
        public decimal Quantity { get; set; }
        public ShipmentQuantityUnits Units { get; set; }
        public int NumberOfPackages { get; set; }
        public IList<PackagingType> PackagingTypes { get; set; }
        public Dictionary<int, Guid> OrderedCarriers { get; set; }
    }
}