namespace EA.Iws.Requests.Admin.Reports
{
    using System;
    using Core.Admin.Reports;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ReportingPermissions.CanViewFinanceReport)]
    public class GetFinanceReport : IRequest<FinanceReportData[]>
    {
        public GetFinanceReport(DateTime endDate)
        {
            EndDate = endDate;
        }

        public DateTime EndDate { get; private set; }
    }
}