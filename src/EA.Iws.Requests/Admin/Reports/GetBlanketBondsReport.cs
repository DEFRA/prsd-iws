namespace EA.Iws.Requests.Admin.Reports
{
    using Core.Admin.Reports;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ReportingPermissions.CanViewBlanketBondsReport)]
    public class GetBlanketBondsReport : IRequest<BlanketBondsData[]>
    {
        public GetBlanketBondsReport(string financialGuaranteeReferenceNumber, string exporterName, string importerName,
            string producerName)
        {
            FinancialGuaranteeReferenceNumber = financialGuaranteeReferenceNumber;
            ExporterName = exporterName;
            ImporterName = importerName;
            ProducerName = producerName;
        }

        public string FinancialGuaranteeReferenceNumber { get; private set; }

        public string ExporterName { get; private set; }

        public string ImporterName { get; private set; }

        public string ProducerName { get; private set; }
    }
}