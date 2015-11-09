namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;

    public class TransitState
    {
        public Guid Id { get; set; }

        public int OrdinalPosition { get; set; }

        public Guid? CountryId { get; set; }

        public Guid? CompetentAuthorityId { get; set; }

        public Guid? EntryPointId { get; set; }

        public Guid? ExitPointId { get; set; }
    }
}
