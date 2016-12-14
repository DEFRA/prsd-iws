namespace EA.Iws.Core.Admin.Reports
{
    using System.ComponentModel;

    public class ExportMovementsData
    {
        [DisplayName("Number of prenotifcations documents uploaded by external user")]
        public int FilesUploadedExternally { get; set; }

        [DisplayName("Number of prenotifications entered in by external user")]
        public int MovementsCreatedExternally { get; set; }

        [DisplayName("Number of prenotifications input by an internal user")]
        public int MovementsCreatedInternally { get; set; }

        [DisplayName("Number of certificates of receipt entered by external user ")]
        public int MovementReceiptsCreatedExternally { get; set; }

        [DisplayName("Number of certificates of receipt entered by internal user")]
        public int MovementReceiptsCreatedInternally { get; set; }

        [DisplayName("Number of certificates of recovery/disposal entered by external user")]
        public int MovementOperationReceiptsCreatedExternally { get; set; }

        [DisplayName("Number of certificates of recovery/disposal entered by internal user")]
        public int MovementOperationReceiptsCreatedInternally { get; set; }
    }
}