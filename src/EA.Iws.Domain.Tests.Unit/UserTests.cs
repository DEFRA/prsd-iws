namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using Core.Admin;
    using TestHelpers.Helpers;
    using Xunit;

    public class UserTests
    {
        private static readonly BusinessType AnyType = BusinessType.LimitedCompany;
        private readonly User anyUser = UserFactory.Create(new Guid("FB282058-6C3C-4B4B-94B4-BDDA2889E89B"), "first", "last", "123", "email@address.com");

        private static readonly Action<User, bool> SetIsAdminForUser =
            (user, isAdmin) => ObjectInstantiator<User>.SetProperty(u => u.IsInternal, isAdmin, user);

        private static readonly Action<User, InternalUserStatus> SetInternalUserStatus =
            (user, status) => ObjectInstantiator<User>.SetProperty(u => u.InternalUserStatus, status, user);

        [Fact]
        public void CanLinkToOrganisation()
        {
            var address = new Address("address1", "address2", "town", "region", "postcode", "country");
            var org = new Organisation("name", address, AnyType, "123");

            anyUser.LinkToOrganisation(org);

            Assert.NotNull(anyUser.Organisation);
        }

        [Fact]
        public void CannotLinkToSecondOrganisation()
        {
            var address = new Address("address1", "address2", "town", "region", "postcode", "country");
            var org = new Organisation("name", address, AnyType, "123");

            anyUser.LinkToOrganisation(org);

            var secondAddress = new Address("address12", "address22", "town2", "region2", "postcode2", "country2");
            var secondOrg = new Organisation("name2", secondAddress, AnyType, "1232");

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