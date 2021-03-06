﻿namespace EA.Iws.Core.Admin.Reports
{
    using System.ComponentModel;

    public class ComplianceDataGuidance
    {
        [DisplayName("Notification number")]
        public string NotificationNumber { get; set; }

        [DisplayName("Number of shipments with No Prenotification")]
        public string NoPrenotificationCount { get; set; }

        [DisplayName("Prenotification")]
        public string PreNotificationColour { get; set; }

        [DisplayName("Number of missing shipments")]
        public string MissingShipments { get; set; }

        [DisplayName("Missing shipments")]
        public string MissingShipmentsColour { get; set; }

        [DisplayName("Number of active shipments over the limit")]
        public string OverLimitShipments { get; set; }

        [DisplayName("Over active loads")]
        public string OverActiveLoads { get; set; }

        [DisplayName("Over quantity (Y/N)")]
        public string OverTonnage { get; set; }

        [DisplayName("Over quantity")]
        public string OverTonnageColour { get; set; }

        [DisplayName("Over shipments (Y/N)")]
        public string OverShipments { get; set; }

        [DisplayName("Over shipments")]
        public string OverShipmentsColour { get; set; }

        [DisplayName("Notifier name")]
        public string Notifier { get; set; }

        [DisplayName("Consignee name")]
        public string Consignee { get; set; }

        [DisplayName("Notification expired (Y/N)")]
        public string FileExpired { get; set; }
    }
}
