namespace EA.Iws.DataAccess.Repositories.Reports
{
    using Core.Notification;
    using Domain.Reports;
    using EA.Iws.Core.Admin.Reports;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    internal class FinanceReportRepository : IFinanceReportRepository
    {
        private readonly IwsContext context;

        public FinanceReportRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Finance>> GetFinanceReport(DateTime from, DateTime to, UKCompetentAuthority competentAuthority)
        {
            return await context.Database.SqlQuery<Finance>(
                @"SELECT
                    [NotificationNumber],
                    [CreatedBy],
                    [Notifier],
                    [NotifierAddress],
                    [NotifierPostalCode],
                    [Consignee],
                    [ConsigneeAddress],
                    [ConsigneePostalCode],
                    [Facility],
                    [FacilityAddress],
                    [FacilityPostalCode],
                    [ReceivedDate],
                    [PaymentReceivedDate],
                    [TotalBillable],
                    [TotalPaid],
                    [LatestPaymentDate],
                    [AmountToRefund],
                    [TotalRefunded],
                    [LatestRefundDate],
                    [IntendedNumberOfShipments],
                    [IntendedQuantity],
                    [Units],
                    [TotalShipmentsMade],
                    [ImportOrExport],
                    [NotificationType],
                    [Preconsented],
                    [HasMultipleFacilities],
                    [ConsentFrom],
                    [ConsentTo],
                    [Status],
                    [IsInterim],
                    [PaymentComments]
                  FROM
                    [Reports].[Finance]
                  WHERE
                    [CompetentAuthorityId] = @ca
                  AND
                    [ReceivedDate] BETWEEN @from AND @to
                  ORDER BY
                    [NotificationNumber]",
                new SqlParameter("@ca", (int)competentAuthority),
                new SqlParameter("@from", from),
                new SqlParameter("@to", to)).ToArrayAsync();
        }

        public async Task<IEnumerable<FinanceReportData>> GetFinanceReport(DateTime from, DateTime to)
        {
            return await context.Database.SqlQuery<FinanceReportData>(
                @"SELECT
                    [NotificationNumber],
                    [CreatedBy],
                    [Notifier],
                    [NotifierAddress],
                    [NotifierPostalCode],
                    [Consignee],
                    [ConsigneeAddress],
                    [ConsigneePostalCode],
                    [Facility],
                    [FacilityAddress],
                    [FacilityPostalCode],
                    [ReceivedDate],
                    [PaymentReceivedDate],
                    [TotalBillable],
                    [TotalPaid],
                    [LatestPaymentDate],
                    [AmountToRefund],
                    [TotalRefunded],
                    [LatestRefundDate],
                    [IntendedNumberOfShipments],
                    [IntendedQuantity],
                    [Units],
                    [TotalShipmentsMade],
                    [ImportOrExport],
                    [NotificationType],
                    [Preconsented],
                    [HasMultipleFacilities],
                    [ConsentFrom],
                    [ConsentTo],
                    [Status],
                    [IsInterim],
                    [PaymentComments]
                  FROM
                    [Reports].[Finance]
                  WHERE
                    [CompetentAuthorityId] = @ca
                  AND
                    [ReceivedDate] BETWEEN @from AND @to
                  ORDER BY
                    [NotificationNumber]",
                new SqlParameter("@ca", (int)UKCompetentAuthority.England),
                new SqlParameter("@from", from),
                new SqlParameter("@to", to)).ToArrayAsync();
        }
    }
}