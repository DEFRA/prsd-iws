namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using TestHelpers.Helpers;
    using Xunit;

    public class CreateNotificationTests
    {
        private readonly IDeferredEventDispatcher dispatcher;
        private readonly Guid notificationId = new Guid("FC532F00-DBEF-4CC2-914C-C1AA218631C6");

        public CreateNotificationTests()
        {
            dispatcher = A.Fake<IDeferredEventDispatcher>();
            DomainEvents.Dispatcher = dispatcher;
        }

        [Fact]
        public void CreateNotificationRaisesDomainEvent()
        {
            var notification = new NotificationApplication(Guid.Empty, NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            EntityHelper.SetEntityId(notification, notificationId);

            A.CallTo(
                () =>
                    dispatcher.Dispatch(
                        A<NotificationCreatedEvent>.That.Matches(p => Equals(p.Notification, notification))))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}