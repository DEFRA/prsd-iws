namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using System.Linq;
    using Core.Admin;
    using Events;
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
        public void Approve_RaisesEvent()
        {
            SetInternalUserStatus(anyUser, InternalUserStatus.Pending);

            anyUser.Approve();

            Assert.Equal(anyUser.User.Email, anyUser.Events.OfType<RegistrationApprovedEvent>().SingleOrDefault().EmailAddress);
        }

        [Fact]
        public void Reject_CanRejectInternalUser()
        {
            SetInternalUserStatus(anyUser, InternalUserStatus.Pending);

            anyUser.Reject();

            Assert.Equal(InternalUserStatus.Rejected, anyUser.Status);
        }

        [Fact]
        public void Reject_RaisesEvent()
        {
            SetInternalUserStatus(anyUser, InternalUserStatus.Pending);

            anyUser.Reject();

            Assert.Equal(anyUser.User.Email, anyUser.Events.OfType<RegistrationRejectedEvent>().SingleOrDefault().EmailAddress);
        }

        [Fact]
        public void UserIdCannotBeNull()
        {
            Action newUser = () => new InternalUser(null, "job title", UKCompetentAuthority.England, new Guid("0A3FAE60-FA0B-4F07-8DB5-2FCDC1B8E66A"));

            Assert.Throws<ArgumentNullException>("userId", newUser);
        }

        [Fact]
        public void UserIdCannotBeEmpty()
        {
            Action newUser = () => new InternalUser(String.Empty, "job title", UKCompetentAuthority.England, new Guid("0A3FAE60-FA0B-4F07-8DB5-2FCDC1B8E66A"));

            Assert.Throws<ArgumentException>("userId", newUser);
        }

        [Fact]
        public void JobTitleCannotBeNull()
        {
            Action newUser = () => new InternalUser("F9437888-8FB6-4E11-A128-BA413176543D", null, UKCompetentAuthority.England, new Guid("0A3FAE60-FA0B-4F07-8DB5-2FCDC1B8E66A"));

            Assert.Throws<ArgumentNullException>("jobTitle", newUser);
        }

        [Fact]
        public void JobTitleCannotBeEmpty()
        {
            Action newUser = () => new InternalUser("F9437888-8FB6-4E11-A128-BA413176543D", string.Empty, UKCompetentAuthority.England, new Guid("0A3FAE60-FA0B-4F07-8DB5-2FCDC1B8E66A"));

            Assert.Throws<ArgumentException>("jobTitle", newUser);
        }

        [Fact]
        public void LocalAreaIdCannotBeDefault()
        {
            Action newUser = () => new InternalUser("F9437888-8FB6-4E11-A128-BA413176543D", "job title", UKCompetentAuthority.England, Guid.Empty);

            Assert.Throws<ArgumentException>("localAreaId", newUser);
        }
    }
}