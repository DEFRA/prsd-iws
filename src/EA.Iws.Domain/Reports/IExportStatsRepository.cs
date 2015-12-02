namespace EA.Iws.Domain.Reports
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IExportStatsRepository
    {
        Task<IEnumerable<ExportStats>> GetExportStats(int year, UKCompetentAuthority competentAuthority);
    }
}