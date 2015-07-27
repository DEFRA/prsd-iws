namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using Core.Admin;
    using TestHelpers.Helpers;
    using Xunit;

    public class UserTests
    {
        private static readonly BusinessType AnyType = BusinessType.LimitedCompany;
        private readonly User anyUser = new User("id", "first", "last", "123", "email@address.com");

        private static readonly Action<User, bool> SetIsAdminForUser =
            (user, isAdmin) => ObjectInstantiator<User>.SetProperty(u => u.IsInternal, isAdmin, user);

        private static readonly Action<User, InternalUserStatus> SetInternalUserStatus =
            (user, status) => ObjectInstantiator<User>.SetProperty(u => u.InternalUserStatus, status, user);

        [Fact]
        public void CanCreateUser()
        {
            var user = new User("id", "first", "last", "123", "email@address.com");

            Assert.NotNull(user);
        }

        [Fact]
        public void IdCannotBeNull()
        {
            Action createUser = () => new User(null, "first", "last", "123", "email@address.com");

            Assert.Throws<ArgumentNullException>("id", createUser);
        }

        [Fact]
        public void IdCannotBeEmpty()
        {
            Action createUser = () => new User(string.Empty, "first", "last", "123", "email@address.com");

            Assert.Throws<ArgumentException>("id", createUser);
        }

        [Fact]
        public void FirstNameCannotBeNull()
        {
            Action createUser = () => new User("id", null, "last", "123", "email@address.com");

            Assert.Throws<ArgumentNullException>("firstName", createUser);
        }

        [Fact]
        public void FirstNameCannotBeEmpty()
        {
            Action createUser = () => new User("id", string.Empty, "last", "123", "email@address.com");

            Assert.Throws<ArgumentException>("firstName", createUser);
        }

        [Fact]
        public void SurnameCannotBeNull()
        {
            Action createUser = () => new User("id", "first", null, "123", "email@address.com");

            Assert.Throws<ArgumentNullException>("surname", createUser);
        }

        [Fact]
        public void SurnameCannotBeEmpty()
        {
            Action createUser = () => new User("id", "first", string.Empty, "123", "email@address.com");

            Assert.Throws<ArgumentException>("surname", createUser);
        }

        [Fact]
        public void PhoneNumberCannotBeNull()
        {
            Action createUser = () => new User("id", "first", "last", null, "email@address.com");

            Assert.Throws<ArgumentNullException>("phoneNumber", createUser);
        }

        [Fact]
        public void PhoneNumberCannotBeEmpty()
        {
            Action createUser = () => new User("id", "first", "last", string.Empty, "email@address.com");

            Assert.Throws<ArgumentException>("phoneNumber", createUser);
        }

        [Fact]
        public void EmailCannotBeNull()
        {
            Action createUser = () => new User("id", "first", "last", "123", null);

            Assert.Throws<ArgumentNullException>("email", createUser);
        }

        [Fact]
        public void EmailCannotBeEmpty()
        {
            Action createUser = () => new User("id", "first", "last", "123", string.Empty);

            Assert.Throws<ArgumentException>("email", createUser);
        }

        [Fact]
        public void CanLinkToOrganisation()
        {
            var address = new Address("building", "address1", "address2", "town", "region", "postcode", "country");
            var org = new Organisation("name", address, AnyType, "123");

            anyUser.LinkToOrganisation(org);

            Assert.NotNull(anyUser.Organisation);
        }

        [Fact]
        public void CannotLinkToSecondOrganisation()
        {
            var address = new Address("building", "address1", "address2", "town", "region", "postcode", "country");
            var org = new Organisation("name", address, AnyType, "123");

            anyUser.LinkToOrganisation(org);

            var secondAddress = new Address("building2", "address12", "address22", "town2", "region2", "postcode2", "country2");
            var secondOrg = new Organisation("name2", secondAddress, AnyType, "1232");

            Action linkToOrganisation = () => anyUser.LinkToOrganisation(secondOrg);

            Assert.Throws<InvalidOperationException>(linkToOrganisation);
        }

        [Fact]
        public void LinkedOrganisationCannotBeNull()
        {
            var user = new User("id", "first", "last", "123", "email@address.com");

            Action linkToOrganisation = () => user.LinkToOrganisation(null);

            Assert.Throws<ArgumentNullException>("organisation", linkToOrganisation);
        }

        [Fact]
        public void Approve_CannotApproveExternalUser()
        {
            SetIsAdminForUser(anyUser, false);

            Assert.Throws<InvalidOperationException>(() => anyUser.Approve());
        }

        [Fact]
        public void Approve_CannotRejectExternalUser()
        {
            SetIsAdminForUser(anyUser, false);

            Assert.Throws<InvalidOperationException>(() => anyUser.Reject());
        }

        [Fact]
        public void Approve_CanApproveInternalUser()
        {
            SetIsAdminForUser(anyUser, true);
            SetInternalUserStatus(anyUser, InternalUserStatus.Pending);

            anyUser.Approve();

            Assert.Equal(InternalUserStatus.Approved, anyUser.InternalUserStatus);
        }

        [Fact]
        public void Reject_CanRejectInternalUser()
        {
            SetIsAdminForUser(anyUser, true);
            SetInternalUserStatus(anyUser, InternalUserStatus.Pending);

            anyUser.Reject();

            Assert.Equal(InternalUserStatus.Rejected, anyUser.InternalUserStatus);
        }
    }
}