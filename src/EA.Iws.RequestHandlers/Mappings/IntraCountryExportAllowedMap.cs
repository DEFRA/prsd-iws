namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shared;
    using Domain;
    using Prsd.Core.Mapper;

    internal class IntraCountryExportAllowedMap : IMap<IntraCountryExportAllowed, IntraCountryExportAllowedData>
    {
        public IntraCountryExportAllowedData Map(IntraCountryExportAllowed source)
        {
            return new IntraCountryExportAllowedData
            {
                ExportCompetentAuthority = (int)source.ExportCompetentAuthority,
                ImportCompetentAuthorityId = source.ImportCompetentAuthorityId
            };
        }
    }
}
