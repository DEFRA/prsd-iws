namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using Core.Shared;
    using Domain.NotificationApplication;
    using TestHelpers.Helpers;
    using Xunit;

    public class UserTests
    {
        private static readonly BusinessType AnyType = BusinessType.LimitedCompany;
        private readonly User anyUser = UserFactory.Create(new Guid("FB282058-6C3C-4B4B-94B4-BDDA2889E89B"), "first", "last", "123", "email@address.com");

        [Fact]
        public void CanLinkToOrganisation()
        {
            var address = new Address("address1", "address2", "town", "region", "postcode", "country");
            var org = new Organisation("name", AnyType, "123");

            anyUser.LinkToOrganisation(org);

            Assert.NotNull(anyUser.Organisation);
        }

        [Fact]
        public void CannotLinkToSecondOrganisation()
        {
            var address = new Address("address1", "address2", "town", "region", "postcode", "country");
            var org = new Organisation("name", AnyType, "123");

            anyUser.LinkToOrganisation(org);

            var secondAddress = new Address("address12", "address22", "town2", "region2", "postcode2", "country2");
            var secondOrg = new Organisation("name2", AnyType, "1232");

            Action linkToOrganisation = () => anyUser.LinkToOrganisation(secondOrg);

            Assert.Throws<InvalidOperationException>(linkToOrganisation);
        }

        [Fact]
        public void LinkedOrganisationCannotBeNull()
        {
            Action linkToOrganisation = () => anyUser.LinkToOrganisation(null);

            Assert.Throws<ArgumentNullException>("organisation", linkToOrganisation);
        }

        [Fact]
        public void LastLoginDate_WhenNotSet_IsNull()
        {
            Assert.Null(anyUser.LastLoginDate);
        }

        [Fact]
        public void LastLoginDate_WhenSet_ReturnsCorrectValue()
        {
            var expectedDate = new DateTime(2026, 3, 3, 10, 30, 0, DateTimeKind.Utc);
            var user = UserFactory.Create(
                new Guid("FB282058-6C3C-4B4B-94B4-BDDA2889E89B"),
                "first",
                "last",
                "123",
                "email@address.com",
                expectedDate);

            Assert.Equal(expectedDate, user.LastLoginDate);
        }

        [Fact]
        public void LastLoginDate_WhenSetToMinValue_ReturnsMinValue()
        {
            var minDate = DateTime.MinValue;
            var user = UserFactory.Create(
                new Guid("FB282058-6C3C-4B4B-94B4-BDDA2889E89B"),
                "first",
                "last",
                "123",
                "email@address.com",
                minDate);

            Assert.Equal(minDate, user.LastLoginDate);
        }

        [Fact]
        public void LastLoginDate_WhenExplicitlySetToNull_IsNull()
        {
            var user = UserFactory.Create(
                new Guid("FB282058-6C3C-4B4B-94B4-BDDA2889E89B"),
                "first",
                "last",
                "123",
                "email@address.com",
                null);

            Assert.Null(user.LastLoginDate);
        }

        [Fact]
        public void UserFactory_CreateWithLastLoginDate_SetsAllProperties()
        {
            var id = new Guid("FB282058-6C3C-4B4B-94B4-BDDA2889E89B");
            var expectedDate = new DateTime(2026, 3, 3, 10, 30, 0, DateTimeKind.Utc);

            var user = UserFactory.Create(id, "first", "last", "123", "email@address.com", expectedDate);

            Assert.Equal(id.ToString(), user.Id);
            Assert.Equal("first", user.FirstName);
            Assert.Equal("last", user.Surname);
            Assert.Equal("123", user.PhoneNumber);
            Assert.Equal("email@address.com", user.Email);
            Assert.Equal(expectedDate, user.LastLoginDate);
        }
    }
}