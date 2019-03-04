namespace EA.Iws.Requests.Admin.Reports
{
    using System;
    using Core.Admin.Reports;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Reports;
    using Core.Reports.Producer;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ReportingPermissions.CanViewProducerReport)]
    public class GetProducerReport : IRequest<ProducerData[]>
    {
        public ProducerReportDates DateType { get; private set; }

        public DateTime From { get; private set; }

        public DateTime To { get; private set; }

        public ProducerReportTextFields? TextFieldType { get; private set; }

        public TextFieldOperator? OperatorType { get; private set; }

        public string TextSearch { get; private set; }

        public GetProducerReport(ProducerReportDates dateType,
            DateTime from,
            DateTime to,
            ProducerReportTextFields? textFieldType,
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
