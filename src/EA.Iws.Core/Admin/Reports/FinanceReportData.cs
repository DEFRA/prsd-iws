namespace EA.Iws.Core.Admin.Reports
{
    using System;
    using System.ComponentModel;

    public class FinanceReportData
    {
        public string NotificationNumber { get; set; }

        public string Notifier { get; set; }

        public string NotifierAddress { get; set; }

        public string NotifierPostalCode { get; set; }

        public decimal? TotalPaid { get; set; }

        public DateTime? LatestPaymentDate { get; set; }

        [DisplayName("Refund Amount")]
        public decimal? TotalRefunded { get; set; }

        [DisplayName("Refund Date")]
        public DateTime? LatestRefundDate { get; set; }

        public int? IntendedNumberOfShipments { get; set; }

        [DisplayName("Actual Shipments Made")]
        public int? TotalShipmentsMade { get; set; }

        [DisplayName("Possible Refund Amount")]
        public decimal? AmountToRefund { get; set; }

        public string NotificationType { get; set; }

        public DateTime? ConsentFrom { get; set; }

        public DateTime? ConsentTo { get; set; }

        public string Status { get; set; }

        [DisplayName("Created By (Internal/External)")]
        public string CreatedBy { get; set; }

        public string Consignee { get; set; }

        public string ConsigneeAddress { get; set; }

        public string ConsigneePostalCode { get; set; }

        public string Facility { get; set; }

        public string FacilityAddress { get; set; }

        public string FacilityPostalCode { get; set; }

        [DisplayName("Notification Received Date")]
        public DateTime? ReceivedDate { get; set; }

        public DateTime? PaymentReceivedDate { get; set; }

        public decimal? TotalBillable { get; set; }
    }
}