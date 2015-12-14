namespace EA.Iws.Requests.Admin.Reports
{
    using Core.Admin.Reports;
    using Prsd.Core.Mediator;

    public class GetMissingShipmentsReport : IRequest<MissingShipmentData[]>
    {
        public int Year { get; private set; }

        public GetMissingShipmentsReport(int year)
        {
            Year = year;
        }
    }
}
