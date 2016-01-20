namespace EA.Iws.Requests.Admin.Reports
{
    using Core.Admin.Reports;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ReportingPermissions.CanViewExportStatsReport)]
    public class GetExportStatsReport : IRequest<ExportStatsData[]>
    {
        public GetExportStatsReport(int year)
        {
            Year = year;
        }

        public int Year { get; private set; }
    }
}