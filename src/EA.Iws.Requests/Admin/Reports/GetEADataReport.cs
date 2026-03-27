namespace EA.Iws.Requests.Admin.Reports
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [RequestAuthorization(ReportingPermissions.CanViewEADataReport)]
    public class GetEADataReport
    {
        public GetEADataReport(DateTime @from, DateTime to)
        {
            From = @from;
            To = to;
        }

        public DateTime From { get; private set; }

        public DateTime To { get; private set; }
    }
}
