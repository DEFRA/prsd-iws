namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Xunit;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public class NotificationSpecialHandlingTests
    {
        private static NotificationApplication CreateNotificationApplication()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                CompetentAuthorityEnum.England, 0);
            return notification;
        }

        [Fact]
        public void CanAddSpecialHandlingDetails()
        {
            var notification = CreateNotificationApplication();

            notification.SetSpecialHandlingRequirements("keep upright");

            Assert.True(notification.HasSpecialHandlingRequirements);
        }

        [Fact]
        public void AddSpecialHandlingSetsDescription()
        {
            var notification = CreateNotificationApplication();

            notification.SetSpecialHandlingRequirements("keep upright");

            Assert.Equal("keep upright", notification.SpecialHandlingDetails);
        }

        [Fact]
        public void CanRemoveSpecialHandlingDetails()
        {
            var notification = CreateNotificationApplication();

            notification.RemoveSpecialHandlingRequirements();

            Assert.False(notification.HasSpecialHandlingRequirements);
        }

        [Fact]
        public void RemoveSpecialHandlingRemovesDescription()
        {
            var notification = CreateNotificationApplication();

            notification.RemoveSpecialHandlingRequirements();

            Assert.Null(notification.SpecialHandlingDetails);
        }

        [Fact]
        public void SpecialHandlingDetailsCantBeNull()
        {
            var notification = CreateNotificationApplication();

            Action setHandling = () => notification.SetSpecialHandlingRequirements(null);

            Assert.Throws<ArgumentNullException>("specialHandlingDetails", setHandling);
        }

        [Fact]
        public void SpecialHandlingDetailsCantBeEmpty()
        {
            var notification = CreateNotificationApplication();

            Action setHandling = () => notification.SetSpecialHandlingRequirements(string.Empty);

            Assert.Throws<ArgumentException>("specialHandlingDetails", setHandling);
        }
    }
}