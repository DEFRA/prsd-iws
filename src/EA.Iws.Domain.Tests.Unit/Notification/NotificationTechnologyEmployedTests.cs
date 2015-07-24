namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using NotificationApplication;
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
            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails("text area contents", "further details"));

            Assert.Equal(notification.TechnologyEmployed.Details, "text area contents");
        }

        [Fact]
        public void AddTechnologyEmployedDetails_AnnexProvidedIsFalse()
        {
            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails("text area contents", "further details"));
            Assert.False(notification.TechnologyEmployed.AnnexProvided);
        }

        [Fact]
        public void CanAddTechnologyDetailsInAnnex()
        {
            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedWithAnnex("details"));

            Assert.True(notification.TechnologyEmployed.AnnexProvided);
        }

        [Fact]
        public void TechnologyDetailsCannotBeNull()
        {
            Action createTechnologyEmployed = () => TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails(null, null);

            Assert.Throws<ArgumentNullException>("details", createTechnologyEmployed);
        }

        [Fact]
        public void TechnologyDetailsCannotBeEmpty()
        {
            Action createTechnologyEmployed = () => TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails(string.Empty, string.Empty);

            Assert.Throws<ArgumentException>("details", createTechnologyEmployed);
        }

        [Fact]
        public void CanUpdateTechnologyEmployedDetails()
        {
            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails("details", "further details"));
            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails("new details", "further details"));

            Assert.Equal("new details", notification.TechnologyEmployed.Details);
        }

        [Fact]
        public void CanUpdateTechnologyEmployedAnnexProvided()
        {
            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails("details", "further details"));
            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedWithAnnex("details"));

            Assert.Equal(true, notification.TechnologyEmployed.AnnexProvided);
        }

        [Fact]
        public void TechnologyDetailsCannotBeMoreThan70CharactersLong()
        {
            const string longString = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Action createTechnologyEmployed = () => TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails(longString, "Details");

            Assert.Throws<InvalidOperationException>(createTechnologyEmployed);
        }
    }
}
