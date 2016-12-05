namespace EA.Iws.Domain.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Core.Reports;
    using Core.WasteType;

    public interface IFreedomOfInformationRepository
    {
        Task<IEnumerable<FreedomOfInformationData>> Get(DateTime from, DateTime to, ChemicalComposition chemicalComposition, UKCompetentAuthority competentAuthority, FoiReportDates dateType);
    }
}