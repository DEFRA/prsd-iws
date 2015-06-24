namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using Domain.Notification;
    using Xunit;

    public class NotificationTechnologyEmployedTests
    {
        private readonly NotificationApplication notification;

        private static NotificationApplication CreateNotification()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Disposal,
                UKCompetentAuthority.England, 0);
            return notification;
        }

        public NotificationTechnologyEmployedTests()
        {
            notification = CreateNotification();
        }

        [Fact]
        public void CanAddTechnologyEmployedDetails()
        {
            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedDetails("text area contents"));

            Assert.Equal(notification.TechnologyEmployed.Details, "text area contents");
        }

        [Fact]
        public void AddTechnologyEmployedDetails_AnnexProvidedIsFalse()
        {
            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedDetails("text area contents"));
            Assert.False(notification.TechnologyEmployed.AnnexProvided);
        }

        [Fact]
        public void CanAddTechnologyDetailsInAnnex()
        {
            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedInAnnex());

            Assert.True(notification.TechnologyEmployed.AnnexProvided);
        }

        [Fact]
        public void TechnologyDetailsCannotBeNull()
        {
            Action createTechnologyEmployed = () => TechnologyEmployed.CreateTechnologyEmployedDetails(null);

            Assert.Throws<ArgumentNullException>("details", createTechnologyEmployed);
        }

        [Fact]
        public void TechnologyDetailsCannotBeEmpty()
        {
            Action createTechnologyEmployed = () => TechnologyEmployed.CreateTechnologyEmployedDetails(string.Empty);

            Assert.Throws<ArgumentException>("details", createTechnologyEmployed);
        }

        [Fact]
        public void CanUpdateTechnologyEmployedDetails()
        {
            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedDetails("details"));
            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedDetails("new details"));

            Assert.Equal("new details", notification.TechnologyEmployed.Details);
        }

        [Fact]
        public void CanUpdateTechnologyEmployedAnnexProvided()
        {
            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedDetails("details"));
            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedInAnnex());

            Assert.Equal(true, notification.TechnologyEmployed.AnnexProvided);
        }
    }
}
