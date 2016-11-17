namespace EA.Iws.Requests.Admin.Reports
{
    using System;
    using Core.Admin.Reports;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ReportingPermissions.CanViewImportNotificationsReport)]
    public class GetImportNotificationsReport : IRequest<DataImportNotificationData[]>
    {
        public DateTime From { get; private set; }

        public DateTime To { get; private set; }
        
        public GetImportNotificationsReport(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }
    }
}
