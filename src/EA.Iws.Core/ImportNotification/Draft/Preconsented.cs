namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;

    public class Preconsented
    {
        public Preconsented(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        protected Preconsented()
        {
        }

        public Guid ImportNotificationId { get; private set; }

        public bool? PreconsentedFacilityExists { get; set; }
    }
}