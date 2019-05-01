namespace EA.Iws.Requests.Admin.Reports
{
    using System;
    using Core.Admin.Reports;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Reports;
    using Core.Reports.Compliance;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ReportingPermissions.CanViewComplianceReport)]
    public class GetComplianceReport : IRequest<ComplianceData[]>
    {
        public ComplianceReportDates DateType { get; private set; }

        public DateTime From { get; private set; }

        public DateTime To { get; private set; }

        public ComplianceTextFields? TextFieldType { get; private set; }

        public TextFieldOperator? OperatorType { get; private set; }

        public string TextSearch { get; private set; }

        public GetComplianceReport(ComplianceReportDates dateType,
            DateTime from,
            DateTime to,
            ComplianceTextFields? textFieldType,
            TextFieldOperator? operatorType,
            string textSearch)
        {
            DateType = dateType;
            From = from;
            To = to;
            TextFieldType = textFieldType;
            OperatorType = operatorType;
            TextSearch = textSearch;
        }
    }
}
