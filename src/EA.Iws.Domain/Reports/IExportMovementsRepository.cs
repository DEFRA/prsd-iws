namespace EA.Iws.Domain.Reports
{
    using System;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;

    public interface IExportMovementsRepository
    {
        Task<ExportMovementsData> Get(DateTime from, DateTime to, UKCompetentAuthority competentAuthority);
    }
}