namespace EA.Iws.Domain.Reports
{
    using System;
    using Core.Shared;

    public class MissingShipment
    {
        public string NotificationNumber { get; protected set; }

        public string Exporter { get; protected set; }

        public string Importer { get; protected set; }

        public string Facility { get; protected set; }

        public int ShipmentNumber { get; protected set; }

        public DateTime? ActualDateOfShipment { get; protected set; }

        public DateTime? ConsentFrom { get; protected set; }

        public DateTime? ConsentTo { get; protected set; }

        public DateTime? PrenotificationDate { get; protected set; }

        public DateTime? ReceivedDate { get; protected set; }

        public DateTime? CompletedDate { get; protected set; }

        public decimal? QuantityReceived { get; protected set; }

        public ShipmentQuantityUnits? Units { get; protected set; }

        public string ChemicalComposition { get; protected set; }

        public string LocalArea { get; protected set; }

        public decimal? TotalQuantity { get; protected set; }

        public ShipmentQuantityUnits? TotalQuantityUnitsId { get; protected set; }

        public string EntryPort { get; protected set; }

        public string DestinationCountry { get; protected set; }

        public string ExitPort { get; protected set; }

        public string OriginatingCountry { get; protected set; }

        public string Status { get; protected set; }
    }
}