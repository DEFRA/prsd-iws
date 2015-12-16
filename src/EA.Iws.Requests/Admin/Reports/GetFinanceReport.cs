namespace EA.Iws.Requests.Admin.Reports
{
    using System;
    using Core.Admin.Reports;
    using Prsd.Core.Mediator;

    public class GetFinanceReport : IRequest<FinanceReportData[]>
    {
        public GetFinanceReport(DateTime endDate)
        {
            EndDate = endDate;
        }

        public DateTime EndDate { get; private set; }
    }
}