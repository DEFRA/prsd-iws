namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using Core.MeansOfTransport;
    using Domain.Notification;
    using Xunit;

    public class NotificationMeansOfTransportTests
    {
        private static NotificationApplication CreateNotificationApplication()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            return notification;
        }

        [Fact]
        public void NotificationWithNoMeans_ReturnsEmptyList()
        {
            var notification = CreateNotificationApplication();

            Assert.Empty(notification.MeansOfTransport);
        }

        [Fact]
        public void NotificationAddMeans_StoresMeans()
        {
            var notification = CreateNotificationApplication();

            var means = new MeansOfTransport[] { MeansOfTransport.Road, MeansOfTransport.Sea, MeansOfTransport.Road };

            notification.SetMeansOfTransport(means);

            Assert.Equal(means, notification.MeansOfTransport);
        }

        [Fact]
        public void NotificationAddMeans_AddNullList_Throws()
        {
            var notification = CreateNotificationApplication();

            Assert.Throws<ArgumentNullException>(() => notification.SetMeansOfTransport(null));
        }

        [Fact]
        public void NotificationAddMeans_AddEmptyList_Throws()
        {
            var notification = CreateNotificationApplication();

            Assert.Throws<InvalidOperationException>(() => notification.SetMeansOfTransport(new MeansOfTransport[0]));
        }

        [Fact]
        public void NotificationAddMeans_OverwritesPreviouslySetValue()
        {
            var notification = CreateNotificationApplication();

            var firstMeans = new[] { MeansOfTransport.Air, MeansOfTransport.Road };
            var secondMeans = new[] { MeansOfTransport.Road, MeansOfTransport.Sea, MeansOfTransport.InlandWaterways };

            notification.SetMeansOfTransport(firstMeans);
            notification.SetMeansOfTransport(secondMeans);

            Assert.Equal(secondMeans, notification.MeansOfTransport);
        }

        [Fact]
        public void NotificationAddMeans_CannotSetTwoOfTheSameMeansAdjacent()
        {
            var notification = CreateNotificationApplication();

            var means = new[] { MeansOfTransport.Road, MeansOfTransport.Road };

            Assert.Throws<InvalidOperationException>(() => notification.SetMeansOfTransport(means));
        }

        [Fact]
        public void NotificationAddMeans_CannotSetTwoOfTheSameMeansAdjacentInASequence()
        {
            var notification = CreateNotificationApplication();

            var means = new[]
            {
                MeansOfTransport.Road, MeansOfTransport.Sea, MeansOfTransport.Road, MeansOfTransport.Air,
                MeansOfTransport.Air
            };

            Assert.Throws<InvalidOperationException>(() => notification.SetMeansOfTransport(means));
        }
    }
}
