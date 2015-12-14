namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class StateOfExport : Entity
    {
        public StateOfExport(Guid exitPointId, Guid competentAuthorityId, Guid countryId)
        {
            Guard.ArgumentNotDefaultValue(() => exitPointId, exitPointId);
            Guard.ArgumentNotDefaultValue(() => competentAuthorityId, competentAuthorityId);
            Guard.ArgumentNotDefaultValue(() => countryId, countryId);

            ExitPointId = exitPointId;
            CompetentAuthorityId = competentAuthorityId;
            CountryId = countryId;
        }

        protected StateOfExport()
        {
        }

        public Guid CountryId { get; private set; }

        public Guid CompetentAuthorityId { get; private set; }

        public Guid ExitPointId { get; private set; }
    }
}