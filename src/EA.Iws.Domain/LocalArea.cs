namespace EA.Iws.Domain
{
    using System;
    using Prsd.Core;

    public class LocalArea
    {
        protected LocalArea()
        {
        }

        public LocalArea(Guid id, string name, int competentAuthorityId)
        {
            Guard.ArgumentNotDefaultValue(() => id, id);
            Guard.ArgumentNotNullOrEmpty(() => name, name);
            Guard.ArgumentNotDefaultValue(() => competentAuthorityId, competentAuthorityId);

            Id = id;
            Name = name;
            CompetentAuthorityId = competentAuthorityId;
        }

        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public int CompetentAuthorityId { get; protected set; }
    }
}
