namespace EA.Iws.Requests.Admin.Reports
{
    using EA.Iws.Core.Admin.Reports;
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.Reports;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Collections.Generic;

    [RequestAuthorization(ReportingPermissions.CanViewEADataReport)]
    public class GetEADataReport : IRequest<EADataReportsData>
    {
        public GetEADataReport(DateTime fromDate, DateTime toDate, List<EAReportList> selectedReportList)
        {
            FromDate = fromDate;
            ToDate = toDate;
            SelectedReportList = selectedReportList;
        }

        public DateTime FromDate { get; private set; }

        public DateTime ToDate { get; private set; }

        public List<EAReportList> SelectedReportList { get; private set; }
    }
}
