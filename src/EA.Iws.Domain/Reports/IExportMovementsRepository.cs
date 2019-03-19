namespace EA.Iws.Domain.Reports
{
    using System;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using EA.Iws.Core.Reports;

    public interface IExportMovementsRepository
    {
        Task<ExportMovementsData> Get(DateTime from, DateTime to, UKCompetentAuthority competentAuthority, OrganisationFilterOptions? organisationFilter, string organisationName);
    }
}