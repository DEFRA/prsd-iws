namespace EA.Iws.Requests.Admin.Reports
{
    using Core.Admin.Reports;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ReportingPermissions.CanViewMissingShipmentsReport)]
    public class GetMissingShipmentsReport : IRequest<MissingShipmentData[]>
    {
        public int Year { get; private set; }

        public GetMissingShipmentsReport(int year)
        {
            Year = year;
        }
    }
}
