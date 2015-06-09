namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using Domain.Notification;
    using Xunit;

    public class NotificationTechnologyEmployedTests
    {
        [Fact]
        public void CannotAddAnnexProvideTrueAndDetailsText()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Disposal, UKCompetentAuthority.England, 0);

            Action updateTechnologyEmployed = () => notification.UpdateTechnologyEmployed(true, "text area contents");

            Assert.Throws<InvalidOperationException>(updateTechnologyEmployed);
        }

        [Fact]
        public void CanAddDetailsWhenAnnexProvidedIsFalse()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Disposal, UKCompetentAuthority.England, 0);

            notification.UpdateTechnologyEmployed(false, "text area contents");

            Assert.Equal(notification.TechnologyEmployed.Details, "text area contents");
        }

        [Fact]
        public void CanAddAnnexProvidedWhenDetailsIsNull()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Disposal, UKCompetentAuthority.England, 0);

            notification.UpdateTechnologyEmployed(true, null);

            Assert.True(notification.TechnologyEmployed.AnnexProvided);
        }

        [Fact]
        public void CanAddAnnexProvidedWhenDetailsIsEmptyString()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Disposal, UKCompetentAuthority.England, 0);

            notification.UpdateTechnologyEmployed(true, string.Empty);

            Assert.True(notification.TechnologyEmployed.AnnexProvided);
        }

        [Fact]
        public void CanNotAddWhenAnnexProvidedIsFalseAndDetailsNull()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Disposal, UKCompetentAuthority.England, 0);

            Action updateTechnologyEmployed = () => notification.UpdateTechnologyEmployed(false, null);

            Assert.Throws<InvalidOperationException>(updateTechnologyEmployed);
        }

        [Fact]
        public void CanNotAddWhenAnnexProvidedIsFalseAndDetailsEmptyString()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Disposal, UKCompetentAuthority.England, 0);

            Action updateTechnologyEmployed = () => notification.UpdateTechnologyEmployed(false, string.Empty);

            Assert.Throws<InvalidOperationException>(updateTechnologyEmployed);
        }
    }
}
