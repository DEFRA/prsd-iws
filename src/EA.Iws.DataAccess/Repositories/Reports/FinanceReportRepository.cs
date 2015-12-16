namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Reports;

    internal class FinanceReportRepository : IFinanceReportRepository
    {
        private readonly IwsContext context;

        public FinanceReportRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Finance>> GetFinanceReport(DateTime endDate)
        {
            return await context.Database.SqlQuery<Finance>(
                @"SELECT
                    [NotificationNumber],
                    [Notifier],
                    [NotifierAddress],
                    [Consignee],
                    [ConsigneeAddress],
                    [Facility],
                    [FacilityAddress],
                    [PaymentReceivedDate],
                    [TotalBillable],
                    [TotalPaid],
                    [LatestPaymentDate],
                    [AmountToRefund],
                    [TotalRefunded],
                    [LatestRefundDate],
                    [IntendedNumberOfShipments],
                    [TotalShipmentsMade],
                    [ImportOrExport],
                    [NotificationType],
                    [Preconsented],
                    [HasMultipleFacilities],
                    [ConsentFrom],
                    [ConsentTo],
                    [Status]                    
                  FROM
                    [Reports].[Finance]").ToArrayAsync();
        }
    }
}