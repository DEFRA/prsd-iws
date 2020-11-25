namespace EA.Iws.Domain
{
    using System;

    public class IntraCountryExportAllowed
    {
        public Guid ExportCompetentAuthorityId { get; private set; }
        public Guid ImportCompetentAuthorityId { get; private set; }

        protected IntraCountryExportAllowed()
        {
        }
    }
}