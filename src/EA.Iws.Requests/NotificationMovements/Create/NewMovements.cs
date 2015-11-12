namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using System.Collections.Generic;
    using Core.PackagingType;
    using Core.Shared;

    public class NewMovements
    {
        public int Count { get; set; }
        public DateTime Date { get; set; }
        public decimal Quantity { get; set; }
        public ShipmentQuantityUnits Units { get; set; }
        public int NumberOfPackages { get; set; }
        public IList<PackagingType> PackagingTypes { get; set; }
        public Dictionary<int, Guid> OrderedCarriers { get; set; }
    }
}