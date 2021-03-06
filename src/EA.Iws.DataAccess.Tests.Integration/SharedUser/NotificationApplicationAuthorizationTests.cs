﻿namespace EA.Iws.DataAccess.Tests.Integration.SharedUser
{
    using System;
    using System.Data.Entity;
    using System.Security;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.Security;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Security;
    using TestHelpers.Helpers;
    using Xunit;
    using NotificationApplicationFactory = TestHelpers.Helpers.NotificationApplicationFactory;

    public class NotificationApplicationAuthorizationTests
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public NotificationApplicationAuthorizationTests()
        {
            var externalUser = Guid.NewGuid();

            userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(externalUser);

            context = new IwsContext(userContext, A.Fake<IEventDispatcher>());
        }

        [Fact]
        public async Task InternalUserSameCompetentAuthorityAccess()
        {
            // Setup.   
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);
            var aspnetInternalUser = UserFactory.Create(Guid.NewGuid(), "Internal", "Internal Last", "12345",
                "abc1111@mail.com");
            var localArea = new LocalArea(Guid.NewGuid(), "Test Area", (int)UKCompetentAuthority.England);

            context.NotificationApplications.Add(notification);
            context.Users.Add(aspnetInternalUser);
            context.LocalAreas.Add(localArea);
            await context.SaveChangesAsync();

            var internalUser = new InternalUser(aspnetInternalUser.Id, "test", UKCompetentAuthority.England,
                localArea.Id);

            context.InternalUsers.Add(internalUser);
            await context.SaveChangesAsync();

            A.CallTo(() => userContext.UserId).Returns(Guid.Parse(internalUser.UserId));

            var authorization = new NotificationApplicationAuthorization(context, userContext);

            // Assert.
            // There's no assertion for 'does not throw exception' so just executing it as normal.
            await authorization.EnsureAccessAsync(notification.Id);

            // Clear data.
            context.DeleteOnCommit(internalUser);
            await context.SaveChangesAsync();

            context.Entry(aspnetInternalUser).State = EntityState.Deleted;
            context.Entry(localArea).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task InternalUserDifferentCompetentAuthorityAccessThrowsException()
        {
            // Setup.   
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);
            var aspnetInternalUser = UserFactory.Create(Guid.NewGuid(), "Internal", "Internal Last", "12345",
                "abc321@mail.com");
            var localArea = new LocalArea(Guid.NewGuid(), "Test Area", (int)UKCompetentAuthority.England);

            context.NotificationApplications.Add(notification);
            context.Users.Add(aspnetInternalUser);
            context.LocalAreas.Add(localArea);
            await context.SaveChangesAsync();

            // Internal user is different UKCA from the notification - should cause the exception.
            var internalUser = new InternalUser(aspnetInternalUser.Id, "test", UKCompetentAuthority.Wales,
                localArea.Id);

            context.InternalUsers.Add(internalUser);
            await context.SaveChangesAsync();

            A.CallTo(() => userContext.UserId).Returns(Guid.Parse(internalUser.UserId));

            var authorization = new NotificationApplicationAuthorization(context, userContext);

            // Assert.
            await Assert.ThrowsAsync<SecurityException>(() => authorization.EnsureAccessAsync(notification.Id));

            // Clear data.
            context.DeleteOnCommit(internalUser);
            await context.SaveChangesAsync();

            context.Entry(aspnetInternalUser).State = EntityState.Deleted;
            context.Entry(localArea).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task RandomExternalUserAccessThrowsException()
        {
            // Setup.
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);
            var aspnetInternalUser = UserFactory.Create(Guid.NewGuid(), "Internal", "Internal Last", "12345",
                "12345678@mail.com");
            var aspnetSharedUser = UserFactory.Create(Guid.NewGuid(), "External", "Shared", "12345",
                "test123456@external.com");
            var localArea = new LocalArea(Guid.NewGuid(), "Test Area", (int)UKCompetentAuthority.England);

            context.NotificationApplications.Add(notification);
            context.Users.Add(aspnetInternalUser);
            context.Users.Add(aspnetSharedUser);
            context.LocalAreas.Add(localArea);
            await context.SaveChangesAsync();

            var internalUser = new InternalUser(aspnetInternalUser.Id, "test", UKCompetentAuthority.England,
                localArea.Id);
            //Shared user is different to the user context.
            var sharedUser = new SharedUser(notification.Id, aspnetSharedUser.Id, DateTimeOffset.Now);

            context.SharedUser.Add(sharedUser);
            await context.SaveChangesAsync();

            context.InternalUsers.Add(internalUser);
            await context.SaveChangesAsync();

            var authorization = new NotificationApplicationAuthorization(context, userContext);

            // Assert.
            await Assert.ThrowsAsync<SecurityException>(() => authorization.EnsureAccessAsync(notification.Id));

            // Clear data.
            context.DeleteOnCommit(internalUser);
            context.DeleteOnCommit(sharedUser);
            await context.SaveChangesAsync();

            context.Entry(aspnetInternalUser).State = EntityState.Deleted;
            context.Entry(aspnetSharedUser).State = EntityState.Deleted;
            context.Entry(localArea).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task SharedUserNotificationAccess()
        {
            // Setup.
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);
            var aspnetInternalUser = UserFactory.Create(Guid.NewGuid(), "Internal", "Internal Last", "12345",
                "internal1234@mail.com");
            var aspnetSharedUser = UserFactory.Create(Guid.NewGuid(), "External", "Shared", "12345",
                "external1234@external.com");
            var localArea = new LocalArea(Guid.NewGuid(), "Test Area", (int)UKCompetentAuthority.England);

            context.NotificationApplications.Add(notification);
            context.Users.Add(aspnetInternalUser);
            context.Users.Add(aspnetSharedUser);
            context.LocalAreas.Add(localArea);
            await context.SaveChangesAsync();

            var internalUser = new InternalUser(aspnetInternalUser.Id, "test", UKCompetentAuthority.England,
                localArea.Id);
            var sharedUser = new SharedUser(notification.Id, aspnetSharedUser.Id, DateTimeOffset.Now);

            context.InternalUsers.Add(internalUser);
            context.SharedUser.Add(sharedUser);
            await context.SaveChangesAsync();

            // Set the shared user to be the user context.
            A.CallTo(() => userContext.UserId).Returns(Guid.Parse(sharedUser.UserId));

            var authorization = new NotificationApplicationAuthorization(context, userContext);

            // Assert.
            // There's no assertion for 'does not throw exception' so just executing it as normal.
            await authorization.EnsureAccessAsync(notification.Id);

            // Clear data.
            context.DeleteOnCommit(internalUser);
            context.DeleteOnCommit(sharedUser);
            await context.SaveChangesAsync();

            context.Entry(aspnetInternalUser).State = EntityState.Deleted;
            context.Entry(aspnetSharedUser).State = EntityState.Deleted;
            context.Entry(localArea).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CheckExternalUserNotOwnerAccessThrowsException()
        {
            // Setup.
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);
            var aspnetInternalUser = UserFactory.Create(Guid.NewGuid(), "Internal", "Internal Last", "12345",
                "eddiejones@mail.com");
            var localArea = new LocalArea(Guid.NewGuid(), "Test Area", (int)UKCompetentAuthority.England);

            context.NotificationApplications.Add(notification);
            context.Users.Add(aspnetInternalUser);
            context.LocalAreas.Add(localArea);
            await context.SaveChangesAsync();

            var internalUser = new InternalUser(aspnetInternalUser.Id, "test", UKCompetentAuthority.England,
                localArea.Id);

            context.InternalUsers.Add(internalUser);
            await context.SaveChangesAsync();

            var authorization = new NotificationApplicationAuthorization(context, userContext);

            // Assert.
            await Assert.ThrowsAsync<SecurityException>(() => authorization.EnsureAccessIsOwnerAsync(notification.Id));

            // Clear data.
            context.DeleteOnCommit(internalUser);
            await context.SaveChangesAsync();

            context.Entry(aspnetInternalUser).State = EntityState.Deleted;
            context.Entry(localArea).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CheckNotificationOwnerAccessDoesNotThrowException()
        {
            // Setup.
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);
            var aspnetInternalUser = UserFactory.Create(Guid.NewGuid(), "Internal", "Internal Last", "12345",
                "internal1234@mail.com");
            var localArea = new LocalArea(Guid.NewGuid(), "Test Area", (int)UKCompetentAuthority.England);

            context.NotificationApplications.Add(notification);
            context.Users.Add(aspnetInternalUser);
            context.LocalAreas.Add(localArea);
            await context.SaveChangesAsync();

            var internalUser = new InternalUser(aspnetInternalUser.Id, "test", UKCompetentAuthority.England,
                localArea.Id);

            context.InternalUsers.Add(internalUser);
            await context.SaveChangesAsync();

            // Set the shared user to be the user context.
            A.CallTo(() => userContext.UserId).Returns(notification.UserId);

            var authorization = new NotificationApplicationAuthorization(context, userContext);

            // Assert.
            // There's no assertion for 'does not throw exception' so just executing it as normal.
            await authorization.EnsureAccessAsync(notification.Id);

            // Clear data.
            context.DeleteOnCommit(internalUser);
            await context.SaveChangesAsync();

            context.Entry(aspnetInternalUser).State = EntityState.Deleted;
            context.Entry(localArea).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }
    }
}
