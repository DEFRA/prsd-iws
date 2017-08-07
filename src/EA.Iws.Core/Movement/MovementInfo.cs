namespace EA.Iws.Core.Movement
{
    using System;
    using System.Collections.Generic;
    using PackagingType;
    using Shared;

    public class MovementInfo
    {
        public Guid Id { get; set; }

        public int ShipmentNumber { get; set; }

        public decimal ActualQuantity { get; set; }

        public ShipmentQuantityUnits Unit { get; set; }

        public IEnumerable<PackagingType> PackagingTypes { get; set; }
    }
}