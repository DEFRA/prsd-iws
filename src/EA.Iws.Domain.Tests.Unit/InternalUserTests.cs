namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using Core.Admin;
    using TestHelpers.Helpers;
    using Xunit;

    public class InternalUserTests
    {
        private static readonly Action<InternalUser, InternalUserStatus> SetInternalUserStatus =
            (user, status) => ObjectInstantiator<InternalUser>.SetProperty(u => u.Status, status, user);

        private readonly InternalUser anyUser =
            InternalUserFactory.Create(new Guid("5F09CFBF-9287-4DDD-9B73-6F9008A7E121"),  UserFactory.Create(new Guid("FB282058-6C3C-4B4B-94B4-BDDA2889E89B"), "first",
                "last", "123", "email@address.com"));

        [Fact]
        public void Approve_CanApproveInternalUser()
        {
            SetInternalUserStatus(anyUser, InternalUserStatus.Pending);

            anyUser.Approve();

            Assert.Equal(InternalUserStatus.Approved, anyUser.Status);
        }

        [Fact]
        public void Reject_CanRejectInternalUser()
        {
            SetInternalUserStatus(anyUser, InternalUserStatus.Pending);

            anyUser.Reject();

            Assert.Equal(InternalUserStatus.Rejected, anyUser.Status);
        }
    }
}