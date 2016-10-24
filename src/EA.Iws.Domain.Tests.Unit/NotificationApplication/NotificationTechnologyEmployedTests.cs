namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using Domain.NotificationApplication;
    using Xunit;

    public class NotificationTechnologyEmployedTests
    {
        private readonly Guid notificationId = new Guid("DBCE5E1B-A535-4693-A808-D2168D157D4F");

        [Fact]
        public void CanAddTechnologyEmployedDetails()
        {
            var technologyEmployed = TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails(notificationId, "text area contents", "further details");

            Assert.Equal(technologyEmployed.Details, "text area contents");
        }

        [Fact]
        public void AddTechnologyEmployedDetails_AnnexProvidedIsFalse()
        {
            var technologyEmployed = TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails(notificationId, "text area contents", "further details");
            Assert.False(technologyEmployed.AnnexProvided);
        }

        [Fact]
        public void CanAddTechnologyDetailsInAnnex()
        {
            var technologyEmployed = TechnologyEmployed.CreateTechnologyEmployedWithAnnex(notificationId, "details");

            Assert.True(technologyEmployed.AnnexProvided);
        }

        [Fact]
        public void TechnologyDetailsCannotBeNull()
        {
            Action createTechnologyEmployed = () => TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails(notificationId, null, null);

            Assert.Throws<ArgumentNullException>("details", createTechnologyEmployed);
        }

        [Fact]
        public void TechnologyDetailsCannotBeEmpty()
        {
            Action createTechnologyEmployed = () => TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails(notificationId, string.Empty, string.Empty);

            Assert.Throws<ArgumentException>("details", createTechnologyEmployed);
        }

        [Fact]
        public void CanUpdateTechnologyEmployedDetails()
        {
            var technologyEmployed = TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails(notificationId, "details", "further details");
            technologyEmployed.SetWithFurtherDetails("new details", "further details");

            Assert.Equal("new details", technologyEmployed.Details);
        }

        [Fact]
        public void CanUpdateTechnologyEmployedAnnexProvided()
        {
            var technologyEmployed = TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails(notificationId, "details", "further details");
            technologyEmployed.SetWithAnnex("details");

            Assert.Equal(true, technologyEmployed.AnnexProvided);
        }

        [Fact]
        public void TechnologyDetailsCannotBeMoreThan70CharactersLong()
        {
            const string longString = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Action createTechnologyEmployed = () => TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails(notificationId, longString, "Details");

            Assert.Throws<InvalidOperationException>(createTechnologyEmployed);
        }
    }
}