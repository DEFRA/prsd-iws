namespace EA.Iws.RequestHandlers.Tests.Unit.Mappings
{
    using Core.Authorization;
    using RequestHandlers.Mappings;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using TestHelpers.Helpers;
    using Xunit;

    public class InternalUserMapTests
    {
        private readonly InternalUserMap mapper;

        public InternalUserMapTests()
        {
            mapper = new InternalUserMap();
        }

        [Fact]
        public void Map_WhenSourceIsNull_ReturnsNull()
        {
            var result = mapper.Map(null);

            Assert.Null(result);
        }

        [Fact]
        public void Map_WhenLastLoginDateIsSet_MapsLastLoginDate()
        {
            var expectedDate = new DateTime(2026, 3, 5, 10, 30, 0, DateTimeKind.Utc);
            var user = UserFactory.Create(
                new Guid("FB282058-6C3C-4B4B-94B4-BDDA2889E89B"),
                "first",
                "last",
                "123",
                "email@address.com",
                expectedDate);
            var internalUser = InternalUserFactory.Create(
                new Guid("5F09CFBF-9287-4DDD-9B73-6F9008A7E121"),
                user);

            var result = mapper.Map(internalUser);

            Assert.Equal(expectedDate, result.LastLoginDate);
        }

        [Fact]
        public void Map_WhenLastLoginDateIsNull_MapsNullLastLoginDate()
        {
            var user = UserFactory.Create(
                new Guid("FB282058-6C3C-4B4B-94B4-BDDA2889E89B"),
                "first",
                "last",
                "123",
                "email@address.com");
            var internalUser = InternalUserFactory.Create(
                new Guid("5F09CFBF-9287-4DDD-9B73-6F9008A7E121"),
                user);

            var result = mapper.Map(internalUser);

            Assert.Null(result.LastLoginDate);
        }

        [Fact]
        public void MapWithClaims_WhenLastLoginDateIsSet_MapsLastLoginDate()
        {
            var expectedDate = new DateTime(2026, 3, 5, 10, 30, 0, DateTimeKind.Utc);
            var user = UserFactory.Create(
                new Guid("FB282058-6C3C-4B4B-94B4-BDDA2889E89B"),
                "first",
                "last",
                "123",
                "email@address.com",
                expectedDate);
            var internalUser = InternalUserFactory.Create(
                new Guid("5F09CFBF-9287-4DDD-9B73-6F9008A7E121"),
                user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, UserRole.Internal.ToString().ToLowerInvariant())
            };

            var result = mapper.Map(internalUser, claims);

            Assert.Equal(expectedDate, result.LastLoginDate);
        }

        [Fact]
        public void Map_MapsAllProperties()
        {
            var expectedDate = new DateTime(2026, 3, 5, 10, 30, 0, DateTimeKind.Utc);
            var user = UserFactory.Create(
                new Guid("FB282058-6C3C-4B4B-94B4-BDDA2889E89B"),
                "first",
                "last",
                "123",
                "email@address.com",
                expectedDate);
            var internalUser = InternalUserFactory.Create(
                new Guid("5F09CFBF-9287-4DDD-9B73-6F9008A7E121"),
                user);

            var result = mapper.Map(internalUser);

            Assert.Equal(internalUser.Id, result.Id);
            Assert.Equal(internalUser.UserId, result.UserId);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.Surname, result.Surname);
            Assert.Equal(user.PhoneNumber, result.PhoneNumber);
            Assert.Equal(expectedDate, result.LastLoginDate);
            Assert.Equal(UserRole.Internal, result.Role);
        }
    }
}