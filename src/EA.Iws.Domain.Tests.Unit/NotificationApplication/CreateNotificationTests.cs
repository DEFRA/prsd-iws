namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using System.Linq;
    using Core.Shared;
    using Domain.NotificationApplication;
    using TestHelpers.Helpers;
    using Xunit;

    public class CreateNotificationTests
    {
        private readonly Guid notificationId = new Guid("FC532F00-DBEF-4CC2-914C-C1AA218631C6");

        [Fact]
        public void CreateNotificationRaisesDomainEvent()
        {
            var notification = new NotificationApplication(Guid.Empty, NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            EntityHelper.SetEntityId(notification, notificationId);

            Assert.Equal(notification,
                notification.Events.OfType<NotificationCreatedEvent>().SingleOrDefault().Notification);
        }
    }
}