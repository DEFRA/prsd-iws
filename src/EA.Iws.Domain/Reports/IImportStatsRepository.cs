namespace EA.Iws.Domain.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification;

    public interface IImportStatsRepository
    {
        Task<IEnumerable<ImportStats>> GetImportStats(DateTime from, DateTime to, UKCompetentAuthority competentAuthority);
    }
}