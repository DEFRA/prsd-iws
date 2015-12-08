namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;
    using System.ComponentModel;

    [DisplayName("Transport route export")]
    public class StateOfExport : IDraftEntity
    {
        public Guid? CountryId { get; set; }

        public Guid? CompetentAuthorityId { get; set; }

        public Guid? ExitPointId { get; set; }

        public Guid ImportNotificationId { get; private set; }

        public StateOfExport(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        internal StateOfExport()
        {
        }
    }
}
