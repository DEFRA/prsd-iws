namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Notification;
    using Domain.Reports;

    internal class FinanceReportRepository : IFinanceReportRepository
    {
        private readonly IwsContext context;

        public FinanceReportRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Finance>> GetFinanceReport(DateTime endDate, UKCompetentAuthority competentAuthority)
        {
            return await context.Database.SqlQuery<Finance>(
                @"SELECT
                    [NotificationNumber],
                    [CreatedBy],
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
                    [Reports].[Finance]
                  WHERE
                    [CompetentAuthorityId] = @ca",
                new SqlParameter("@ca", (int)competentAuthority)).ToArrayAsync();
        }
    }
}