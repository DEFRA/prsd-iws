namespace EA.Iws.Domain.Reports
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public interface IExportStatsRepository
    {
        Task<IEnumerable<ExportStats>> GetExportStats(int year, CompetentAuthorityEnum competentAuthority);
    }
}