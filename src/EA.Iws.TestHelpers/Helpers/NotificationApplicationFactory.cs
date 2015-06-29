namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using Domain;
    using Domain.Notification;

    public class NotificationApplicationFactory
    {
        public static NotificationApplication Create(Guid id)
        {
            var notificationApplication = new NotificationApplication(Guid.Empty, NotificationType.Recovery,
                UKCompetentAuthority.England, 250);

            EntityHelper.SetEntityId(notificationApplication, id);

            return notificationApplication;
        }
    }
}
