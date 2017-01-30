namespace EA.Iws.Domain.Reports
{
    using System;

    public class Finance
    {
        public string NotificationNumber { get; protected set; }

        public string CreatedBy { get; set; }

        public string Notifier { get; protected set; }

        public string NotifierAddress { get; protected set; }

        public string NotifierPostalCode { get; set; }

        public string Consignee { get; protected set; }

        public string ConsigneeAddress { get; protected set; }

        public string ConsigneePostalCode { get; set; }

        public string Facility { get; protected set; }

        public string FacilityAddress { get; protected set; }

        public string FacilityPostalCode { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public DateTime? PaymentReceivedDate { get; protected set; }

        public decimal? TotalBillable { get; protected set; }

        public decimal? TotalPaid { get; protected set; }

        public DateTime? LatestPaymentDate { get; protected set; }

        public decimal? AmountToRefund { get; protected set; }

        public decimal? TotalRefunded { get; protected set; }

        public DateTime? LatestRefundDate { get; protected set; }

        public int? IntendedNumberOfShipments { get; protected set; }

        public int? TotalShipmentsMade { get; protected set; }

        public string ImportOrExport { get; protected set; }

        public string NotificationType { get; protected set; }

        public bool? Preconsented { get; protected set; }

        public bool HasMultipleFacilities { get; protected set; }

        public DateTime? ConsentFrom { get; protected set; }

        public DateTime? ConsentTo { get; protected set; }

        public string Status { get; protected set; }
    }
}