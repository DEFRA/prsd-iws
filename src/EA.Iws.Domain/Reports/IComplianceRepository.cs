namespace EA.Iws.Domain.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Core.Reports;
    using EA.Iws.Core.Reports.Compliance;
    public interface IComplianceRepository
    {
        Task<IEnumerable<ComplianceData>> GetComplianceReport(ComplianceReportDates dateType, DateTime from, DateTime to,
           ComplianceTextFields? textFieldType, TextFieldOperator? operatorType, string textSearch,
           UKCompetentAuthority competentAuthority);
    }
}
