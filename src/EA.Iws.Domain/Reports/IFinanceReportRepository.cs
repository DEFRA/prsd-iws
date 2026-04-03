namespace EA.Iws.Domain.Reports
{
    using Core.Notification;
    using EA.Iws.Core.Admin.Reports;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFinanceReportRepository
    {
        Task<IEnumerable<Finance>> GetFinanceReport(DateTime from, DateTime to, UKCompetentAuthority competentAuthority);

        Task<IEnumerable<FinanceReportData>> GetFinanceReport(DateTime from, DateTime to);
    }
}