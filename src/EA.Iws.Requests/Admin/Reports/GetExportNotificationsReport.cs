namespace EA.Iws.Requests.Admin.Reports
{
    using System;
    using Core.Admin.Reports;
    using Prsd.Core.Mediator;

    public class GetExportNotificationsReport : IRequest<DataExportNotificationData[]>
    {
        public DateTime From { get; private set; }

        public DateTime To { get; private set; }
        
        public GetExportNotificationsReport(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }
    }
}
