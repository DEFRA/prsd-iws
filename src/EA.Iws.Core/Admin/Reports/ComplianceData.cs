namespace EA.Iws.Core.Admin.Reports
{
    using System.ComponentModel;
    public class ComplianceData
    {
        public string NotificationNumber { get; set; }

        [DisplayName("Number of shipments with No Pre-notification")]
        public int NoPrenotificationCount { get; set; }

        [DisplayName("Pre-notification")]
        public string PreNotificationColour { get; set; }

        [DisplayName("Number of missing shipments")]
        public int MissingShipments { get; set; }

        [DisplayName("Missing Shipments")]
        public string MissingShipmentsColour { get; set; }

        [DisplayName("Number of active shipments over the limit")]
        public string OverLimitShipments { get; set; }

        [DisplayName("Over Active Loads")]
        public string OverActiveLoads { get; set; }

        [DisplayName("Over Tonnage (Y/N)")]
        public string OverTonnage { get; set; }

        [DisplayName("Over Tonnage")]
        public string OverTonnageColour { get; set; }

        [DisplayName("Over Shipments (Y/N)")]
        public string OverShipments { get; set; }

        [DisplayName("Over Shipments")]
        public string OverShipmentsColour { get; set; }

        [DisplayName("Notifier name")]
        public string Notifier { get; set; }

        [DisplayName("Consignee name")]
        public string Consignee { get; set; }

        [DisplayName("File expired (Y/N)")]
        public string FileExpired { get; set; }
    }   
}