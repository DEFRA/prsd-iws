namespace EA.Iws.Core.Reports
{
    using System.ComponentModel.DataAnnotations;
    public enum ShipmentReportOutputColumns
    {
        [Display(Name = "Import/Export")]
        ImportOrExport = 2,

        [Display(Name = "Notifier")]
        Exporter,

        [Display(Name = "Consignee")]
        Importer,

        Facility,

        [Display(Name = "Basel/OECD Code and Description")]
        BaselOecdCode,

        [Display(Name = "Shipment Number")]
        ShipmentNumber,

        [Display(Name = "Actual Date Of Shipment")]
        ActualDateOfShipment,

        [Display(Name = "Consent Valid From")]
        ConsentValidFrom,

        [Display(Name = "Consent Valid To")]
        ConsentValidTo,

        [Display(Name = "Prenotification Date")]
        PrenotificationDate,

        [Display(Name = "Shipment Received Date")]
        ReceivedDate,

        [Display(Name = "Recovery Or Disposal Date")]
        RecoveryOrDisposalDate,

        [Display(Name = "Cancelled Shipment")]
        CancelledShipment,

        [Display(Name = "Quantity Received (Cubic Metres)")]
        CubicMetresQuantityReceived,

        [Display(Name = "Quantity Received (Tonnes)")]
        TonnesQuantityReceived,

        [Display(Name = "Waste Type / Name of Waste")]
        ChemicalComposition,

        [Display(Name = "Area")]
        CompetentAuthorityArea,

        [Display(Name = "Intended Quantity (Cubic Metres)")]
        IntendedCubicMetresQuantity,

        [Display(Name = "Intended Quantity (Tonnes)")]
        IntendedTonnesQuantity,

        [Display(Name = "Point of Exit")]
        PortOfExit,

        [Display(Name = "Country of Dispatch")]
         DispatchingCountry,

        [Display(Name = "Point of Entry")]
        PortOfEntry,

        [Display(Name = "Country of Destination")]
        DestinationCountry,

        [Display(Name = "Prenotified on Time")]
        WasPrenotifiedBeforeThreeWorkingDays,

        [Display(Name = "EWC Codes")]
        EwcCodes,

        [Display(Name = "R/D Code(s)")]
        OperationCodes,

        [Display(Name = "Y Code")]
        YCode,

        [Display(Name = "H Code")]
        HCode,

        [Display(Name = "UN Class")]
        UNClass,

        [Display(Name = "Notification status")]
        NotificationStatus,
    }
}
