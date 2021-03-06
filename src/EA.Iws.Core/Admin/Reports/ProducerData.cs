﻿namespace EA.Iws.Core.Admin.Reports
{
    using System.ComponentModel;

    public class ProducerData
    {
        public string NotificationNumber { get; set; }

        [DisplayName("Notifier name")]
        public string NotifierName { get; set; }

        [DisplayName("Producer name")]
        public string ProducerName { get; set; }

        [DisplayName("Producer address 1")]
        public string ProducerAddress1 { get; set; }

        [DisplayName("Producer address 2")]
        public string ProducerAddress2 { get; set; }

        [DisplayName("Producer Town or City")]
        public string ProducerTownOrCity { get; set; }

        [DisplayName("Producer postcode")]
        public string ProducerPostcode { get; set; }
        
        [DisplayName("Site of export/Postcode")]
        public string SiteOfExport { get; set; }

        [DisplayName("Area")]
        public string LocalArea { get; set; }

        [DisplayName("Waste type")]
        public string WasteType { get; set; }

        [DisplayName("Notification status")]
        public string NotificationStatus { get; set; }

        [DisplayName("Consignee name")]
        public string ConsigneeName { get; set; }
    }
}
