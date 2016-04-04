namespace EA.Iws.Domain.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.WasteType;

    public interface IRdfSrfWoodRepository
    {
        Task<IEnumerable<RdfSrfWoodData>> Get(DateTime from, DateTime to, ChemicalComposition chemicalComposition);
    }
}