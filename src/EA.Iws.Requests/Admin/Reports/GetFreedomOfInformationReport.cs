namespace EA.Iws.Requests.Admin.Reports
{
    using System;
    using Core.Admin.Reports;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Reports;
    using Core.Reports.FOI;
    using Core.WasteType;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ReportingPermissions.CanViewFoiReport)]
    public class GetFreedomOfInformationReport : IRequest<FreedomOfInformationData[]>
    {
        public GetFreedomOfInformationReport(DateTime from, DateTime to, FOIReportDates dateType, FOIReportTextFields? searchField,
            TextFieldOperator? comparisonType,
            string searchText)
        {
            From = from;
            To = to;
            DateType = dateType;
            SearchField = searchField;
            ComparisonType = comparisonType;
            SearchText = searchText;
        }

        public DateTime From { get; private set; }

        public DateTime To { get; private set; }

        public FOIReportDates DateType { get; private set; }

        public FOIReportTextFields? SearchField { get; private set; }

        public TextFieldOperator? ComparisonType { get; private set; }

        public string SearchText { get; private set; }
    }
}