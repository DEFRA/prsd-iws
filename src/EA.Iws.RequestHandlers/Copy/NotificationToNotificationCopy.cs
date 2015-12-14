namespace EA.Iws.RequestHandlers.Copy
{
    using System;
    using Domain.NotificationApplication;
    using Prsd.Core.Domain;

    internal class NotificationToNotificationCopy
    {
        private readonly WasteCodeCopy wasteCodeCopy;

        public NotificationToNotificationCopy(WasteCodeCopy wasteCodeCopy)
        {
            this.wasteCodeCopy = wasteCodeCopy;
        }

        public virtual void CopyNotificationProperties(NotificationApplication source,
            NotificationApplication destination)
        {
            // We want to set all properties except a few decided by business logic.
            typeof(NotificationApplication).GetProperty("NotificationNumber")
                .SetValue(source, destination.NotificationNumber, null);
            typeof(NotificationApplication).GetProperty("CompetentAuthority")
                .SetValue(source, destination.CompetentAuthority, null);
            typeof(NotificationApplication).GetProperty("NotificationType")
                .SetValue(source, destination.NotificationType, null);
            typeof(NotificationApplication).GetProperty("Charge")
                .SetValue(source, destination.Charge, null);

            // This should not be needed however is a precaution to prevent overwriting the source data.
            typeof(Entity).GetProperty("Id").SetValue(source, Guid.Empty, null);
        }

        public virtual void CopyLookupEntities(NotificationApplication source, NotificationApplication destination)
        {
            CopyWasteCodes(source, destination);
        }

        protected virtual void CopyWasteCodes(NotificationApplication source, NotificationApplication destination)
        {
            wasteCodeCopy.CopyWasteCodes(source, destination);
        }
    }
}