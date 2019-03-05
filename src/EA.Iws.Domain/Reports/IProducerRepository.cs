namespace EA.Iws.Domain.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Core.Reports;

    public interface IProducerRepository
    {
        Task<IEnumerable<ProducerData>> GetProducerReport(ProducerReportDates dateType, DateTime from, DateTime to,
            ProducerReportTextFields? textFieldType, TextFieldOperator? operatorType, string textSearch,
            UKCompetentAuthority competentAuthority);
    }
}
