namespace EA.Iws.Domain.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification;

    public interface IFinanceReportRepository
    {
        Task<IEnumerable<Finance>> GetFinanceReport(DateTime endDate, UKCompetentAuthority competentAuthority);
    }
}