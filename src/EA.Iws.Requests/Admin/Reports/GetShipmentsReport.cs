namespace EA.Iws.Requests.Admin.Reports
{
    using System;
    using Core.Admin.Reports;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Reports;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ReportingPermissions.CanViewMissingShipmentsReport)]
    public class GetShipmentsReport : IRequest<ShipmentData[]>
    {
        public DateTime From { get; private set; }

        public DateTime To { get; private set; }

        public ShipmentsReportDates DateType { get; private set; }

        public GetShipmentsReport(DateTime from, DateTime to, ShipmentsReportDates dateType)
        {
            From = from;
            To = to;
            DateType = dateType;
        }
    }
}
