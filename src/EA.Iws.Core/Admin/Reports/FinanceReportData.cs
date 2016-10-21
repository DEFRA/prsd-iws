namespace EA.Iws.Core.Admin.Reports
{
    using System;

    public class FinanceReportData
    {
        public string NotificationNumber { get; set; }

        public string CreatedBy { get; set; }

        public string Notifier { get; set; }

        public string NotifierAddress { get; set; }

        public string Consignee { get; set; }

        public string ConsigneeAddress { get; set; }

        public string Facility { get; set; }

        public string FacilityAddress { get; set; }

        public DateTime? PaymentReceivedDate { get; set; }

        public decimal? TotalBillable { get; set; }

        public decimal? TotalPaid { get; set; }

        public DateTime? LatestPaymentDate { get; set; }

        public decimal? AmountToRefund { get; set; }

        public decimal? TotalRefunded { get; set; }

        public DateTime? LatestRefundDate { get; set; }

        public int? IntendedNumberOfShipments { get; set; }

        public int? TotalShipmentsMade { get; set; }

        public string NotificationType { get; set; }

        public DateTime? ConsentFrom { get; set; }

        public DateTime? ConsentTo { get; set; }

        public string Status { get; set; }
    }
}