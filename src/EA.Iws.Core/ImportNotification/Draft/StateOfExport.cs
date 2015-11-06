namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;

    public class StateOfExport
    {
        public Guid? CountryId { get; set; }

        public Guid? CompetentAuthorityId { get; set; }

        public Guid? ExitPointId { get; set; }
    }
}
