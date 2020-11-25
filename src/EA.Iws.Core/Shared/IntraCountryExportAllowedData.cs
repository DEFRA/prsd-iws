namespace EA.Iws.Core.Shared
{
    using System;

    public class IntraCountryExportAllowedData
    {
        public Guid ExportCompetentAuthorityId { get; set; }
        public Guid ImportCompetentAuthorityId { get; set; }
    }
}