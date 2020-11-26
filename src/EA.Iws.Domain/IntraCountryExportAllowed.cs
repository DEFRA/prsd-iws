namespace EA.Iws.Domain
{
    using System;

    public class IntraCountryExportAllowed
    {
        public Guid ExportCompetentAuthorityId { get; protected set; }
        public Guid ImportCompetentAuthorityId { get; protected set; }

        protected IntraCountryExportAllowed()
        {
        }
    }
}