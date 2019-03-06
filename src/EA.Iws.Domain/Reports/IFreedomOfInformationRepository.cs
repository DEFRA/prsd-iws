namespace EA.Iws.Domain.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Core.Reports;
    using Core.Reports.FOI;

    public interface IFreedomOfInformationRepository
    {
        Task<IEnumerable<FreedomOfInformationData>> Get(DateTime from, DateTime to, UKCompetentAuthority competentAuthority, FOIReportDates dateType, FOIReportTextFields? searchField,
            TextFieldOperator? searchType,
            string comparisonText);
    }
}