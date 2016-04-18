namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class TransitState : Entity
    {
        public TransitState(Guid entryPointId, Guid exitPointId, Guid countryId,
            Guid competentAuthorityId, int ordinalPosition)
        {
            Guard.ArgumentNotDefaultValue(() => entryPointId, entryPointId);
            Guard.ArgumentNotDefaultValue(() => exitPointId, exitPointId);
            Guard.ArgumentNotDefaultValue(() => countryId, countryId);
            Guard.ArgumentNotDefaultValue(() => competentAuthorityId, competentAuthorityId);
            Guard.ArgumentNotZeroOrNegative(() => ordinalPosition, ordinalPosition);

            CompetentAuthorityId = competentAuthorityId;
            CountryId = countryId;
            EntryPointId = entryPointId;
            ExitPointId = exitPointId;
            OrdinalPosition = ordinalPosition;
        }

        protected TransitState()
        {
        }

        public int OrdinalPosition { get; private set; }

        public Guid CountryId { get; private set; }

        public Guid CompetentAuthorityId { get; private set; }

        public Guid EntryPointId { get; private set; }

        public Guid ExitPointId { get; private set; }
    }
}