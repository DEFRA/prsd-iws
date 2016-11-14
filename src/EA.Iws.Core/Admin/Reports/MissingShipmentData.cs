namespace EA.Iws.Core.Admin.Reports
{
    using System;

    public class MissingShipmentData
    {
        public string NotificationNumber { get; set; }

        public string Exporter { get; set; }

        public string Importer { get; set; }

        public string Facility { get; set; }

        public int ShipmentNumber { get; set; }

        public DateTime? ActualDateOfShipment { get; set; }

        public DateTime? ConsentValidFrom { get; set; }

        public DateTime? ConsentValidTo { get; set; }

        public DateTime? PrenotificationDate { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public DateTime? RecoveryOrDisposalDate { get; set; }

        public string CancelledShipment { get; set; }

        public decimal? CubicMetresQuantityReceived { get; set; }

        public decimal? TonnesQuantityReceived { get; set; }

        public string ChemicalComposition { get; set; }

        public string CompetentAuthorityArea { get; set; }

        public bool WasPrenotifiedBeforeThreeWorkingDays { get; set; }

        public decimal? IntendedCubicMetresQuantity { get; set; }

        public decimal? IntendedTonnesQuantity { get; set; }

        public string PortOfExit { get; set; }

        public string DispatchingCountry { get; set; }

        public string PortOfEntry { get; set; }

        public string DestinationCountry { get; set; }
    }
}
