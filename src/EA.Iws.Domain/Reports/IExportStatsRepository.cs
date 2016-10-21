namespace EA.Iws.Domain.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification;

    public interface IExportStatsRepository
    {
        Task<IEnumerable<ExportStats>> GetExportStats(DateTime from, DateTime to, UKCompetentAuthority competentAuthority);
    }
}