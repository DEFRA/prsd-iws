namespace EA.Iws.Core.Admin.Reports
{
    using System;
    using System.ComponentModel;

    public class ShipmentData
    {
        public string NotificationNumber { get; set; }

        [DisplayName("Import/Export")]
        public string ImportOrExport { get; set; }

        [DisplayName("Notifier")]
        public string Exporter { get; set; }

        [DisplayName("Consignee")]
        public string Importer { get; set; }

        public string Facility { get; set; }

        [DisplayName("Basel/OECD Code and Description")]
        public string BaselOecdCode { get; set; }

        public int ShipmentNumber { get; set; }

        public DateTime? ActualDateOfShipment { get; set; }

        public DateTime? ConsentValidFrom { get; set; }

        public DateTime? ConsentValidTo { get; set; }

        public DateTime? PrenotificationDate { get; set; }

        [DisplayName("Shipment Received Date")]
        public DateTime? ReceivedDate { get; set; }

        public DateTime? RecoveryOrDisposalDate { get; set; }

        public string CancelledShipment { get; set; }

        [DisplayName("Quantity Received (Cubic Metres)")]
        public decimal? CubicMetresQuantityReceived { get; set; }

        [DisplayName("Quantity Received (Tonnes)")]
        public decimal? TonnesQuantityReceived { get; set; }

        [DisplayName("Waste Type / Name of Waste")]
        public string ChemicalComposition { get; set; }

        [DisplayName("Area")]
        public string CompetentAuthorityArea { get; set; }

        [DisplayName("Intended Quantity (Cubic Metres)")]
        public decimal? IntendedCubicMetresQuantity { get; set; }

        [DisplayName("Intended Quantity (Tonnes)")]
        public decimal? IntendedTonnesQuantity { get; set; }

        [DisplayName("Point of Exit")]
        public string PortOfExit { get; set; }

        [DisplayName("Country of Dispatch")]
        public string DispatchingCountry { get; set; }

        [DisplayName("Point of Entry")]
        public string PortOfEntry { get; set; }

        [DisplayName("Country of Destination")]
        public string DestinationCountry { get; set; }

        [DisplayName("Prenotified on Time")]
        public bool WasPrenotifiedBeforeThreeWorkingDays { get; set; }

        [DisplayName("EWC Codes")]
        public string EwcCodes { get; set; }

        [DisplayName("R/D Code(s)")]
        public string OperationCodes { get; set; }

        [DisplayName("Y Code")]
        public string YCode { get; set; }

        [DisplayName("H Code")]
        public string HCode { get; set; }

        [DisplayName("UN Class")]
        public string UNClass { get; set; }
    }
}
