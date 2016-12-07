namespace EA.Iws.Requests.Admin.Reports
{
    using System;
    using Core.Admin.Reports;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Reports;
    using Core.WasteType;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ReportingPermissions.CanViewFoiReport)]
    public class GetFreedomOfInformationReport : IRequest<FreedomOfInformationData[]>
    {
        public GetFreedomOfInformationReport(DateTime from, DateTime to, ChemicalComposition chemicalComposition, FoiReportDates dateType)
        {
            From = from;
            To = to;
            ChemicalComposition = chemicalComposition;
            DateType = dateType;
        }

        public DateTime From { get; private set; }

        public DateTime To { get; private set; }

        public ChemicalComposition ChemicalComposition { get; private set; }

        public FoiReportDates DateType { get; private set; }
    }
}