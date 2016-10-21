namespace EA.Iws.RequestHandlers.Mappings.Reports
{
    using System.Text;
    using Core.Admin.Reports;
    using Domain.Reports;
    using Prsd.Core.Mapper;

    internal class FinanceMap : IMap<Finance, FinanceReportData>
    {
        public FinanceReportData Map(Finance source)
        {
            return new FinanceReportData
            {
                AmountToRefund = source.AmountToRefund,
                ConsentFrom = source.ConsentFrom,
                ConsentTo = source.ConsentTo,
                Consignee = source.Consignee,
                ConsigneeAddress = source.ConsigneeAddress,
                CreatedBy = source.CreatedBy,
                Facility = source.Facility,
                FacilityAddress = source.FacilityAddress,
                IntendedNumberOfShipments = source.IntendedNumberOfShipments,
                LatestPaymentDate = source.LatestPaymentDate,
                LatestRefundDate = source.LatestRefundDate,
                NotificationType = GetNotificationType(source),
                Status = source.Status,
                NotificationNumber = source.NotificationNumber,
                Notifier = source.Notifier,
                NotifierAddress = source.NotifierAddress,
                PaymentReceivedDate = source.PaymentReceivedDate,
                TotalBillable = source.TotalBillable,
                TotalPaid = source.TotalPaid,
                TotalRefunded = source.TotalRefunded,
                TotalShipmentsMade = source.TotalShipmentsMade
            };
        }

        private static string GetNotificationType(Finance source)
        {
            var type = new StringBuilder();

            type.Append(source.ImportOrExport);
            type.Append(" for ");
            type.Append(source.NotificationType);

            if (source.Preconsented.GetValueOrDefault())
            {
                type.Append(" (Pre-consented)");
            }

            type.Append(source.HasMultipleFacilities ? " (Interim)" : " (Non-interim)");

            return type.ToString();
        }
    }
}