namespace EA.Iws.Requests.Admin.Reports
{
    using Core.Admin.Reports;
    using Core.Authorization;
    using Prsd.Core.Mediator;

    [RequestAuthorization("Get Export Stats Report")]
    public class GetExportStatsReport : IRequest<ExportStatsData[]>
    {
        public GetExportStatsReport(int year)
        {
            Year = year;
        }

        public int Year { get; private set; }
    }
}