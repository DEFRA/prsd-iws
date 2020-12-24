namespace EA.Iws.Domain
{
    using EA.Iws.Core.Notification;
    using System;

    public class IntraCountryExportAllowed
    {
        public UKCompetentAuthority ExportCompetentAuthority { get; protected set; }

        public Guid ImportCompetentAuthorityId { get; protected set; }

        protected IntraCountryExportAllowed()
        {
        }
    }
}