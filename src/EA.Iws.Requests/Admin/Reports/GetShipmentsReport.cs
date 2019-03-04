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

        public ShipmentReportTextFields? TextFieldType { get; private set; }

        public TextFieldOperator? TextFieldOperatorType { get; private set; }

        public string TextSearch { get; private set; }

        public GetShipmentsReport(DateTime from, DateTime to, ShipmentsReportDates dateType,
            ShipmentReportTextFields? textFieldType, TextFieldOperator? textFieldOperatorType,
            string textSearch)
        {
            From = from;
            To = to;
            DateType = dateType;
            TextFieldType = textFieldType;
            TextFieldOperatorType = textFieldOperatorType;
            TextSearch = textSearch;
        }
    }
}
