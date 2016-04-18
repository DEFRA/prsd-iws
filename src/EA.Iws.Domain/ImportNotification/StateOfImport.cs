namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class StateOfImport : Entity
    {
        public StateOfImport(Guid entryPointId, Guid competentAuthorityId)
        {
            Guard.ArgumentNotDefaultValue(() => entryPointId, entryPointId);
            Guard.ArgumentNotDefaultValue(() => competentAuthorityId, competentAuthorityId);

            EntryPointId = entryPointId;
            CompetentAuthorityId = competentAuthorityId;
        }

        protected StateOfImport()
        {
        }

        public Guid CompetentAuthorityId { get; private set; }

        public Guid EntryPointId { get; private set; }
    }
}