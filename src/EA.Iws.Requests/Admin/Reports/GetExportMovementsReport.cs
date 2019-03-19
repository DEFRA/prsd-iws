namespace EA.Iws.Requests.Admin.Reports
{
    using System;
    using Core.Admin.Reports;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using EA.Iws.Core.Reports;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ReportingPermissions.CanViewExportMovementsReport)]
    public class GetExportMovementsReport : IRequest<ExportMovementsData>
    {
        public DateTime From { get; private set; }

        public DateTime To { get; private set; }

        public OrganisationFilterOptions? OrganisationFilter { get; private set; }

        public string OrganisationName { get; private set; }

        public GetExportMovementsReport(DateTime from, DateTime to, OrganisationFilterOptions? organisationFilter, string organisationName)
        {
            From = from;
            To = to;
            OrganisationFilter = organisationFilter;
            OrganisationName = organisationName;
        }
    }
}