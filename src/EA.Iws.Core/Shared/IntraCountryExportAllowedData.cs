namespace EA.Iws.Core.Shared
{
    using System;

    public class IntraCountryExportAllowedData
    {
        public int ExportCompetentAuthority { get; set; }
        public Guid ImportCompetentAuthorityId { get; set; }
    }
}