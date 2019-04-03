namespace EA.Iws.DataAccess.Tests.Integration.SharedUser
{
    using System;
    using System.Data.Entity;
    using System.Security;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Security;
    using TestHelpers.Helpers;
    using Xunit;
    using NotificationApplicationFactory = TestHelpers.Helpers.NotificationApplicationFactory;

    public class IwsContextTests
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public IwsContextTests()
        {
            var externalUser = Guid.NewGuid();

            userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(externalUser);

            context = new IwsContext(userContext, A.Fake<IEventDispatcher>());
        }

        [Fact]
        public async Task RandomExternalUserAccessThrowsException()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);
            var aspnetInternalUser = UserFactory.Create(Guid.NewGuid(), "Internal", "Internal Last", "12345",
                "abc@mail.com");
            var aspnetSharedUser = UserFactory.Create(Guid.NewGuid(), "External", "Shared", "12345",
                "12345@external.com");
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

            await Assert.ThrowsAsync<SecurityException>(() => context.GetNotificationApplication(notification.Id));

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
        public async Task SharedUserAccessDoesNotThrowException()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);
            var aspnetInternalUser = UserFactory.Create(Guid.NewGuid(), "Internal", "Internal Last", "12345",
                "internal123@mail.com");
            var aspnetSharedUser = UserFactory.Create(Guid.NewGuid(), "External", "Shared", "12345",
                "external123@external.com");
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

            // There's no assertion for 'does not throw exception' so just executing it as normal.
            await authorization.EnsureAccessAsync(notification.Id);

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
        public async Task InternalUserAccessDoesNotThrowException()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);
            var aspnetInternalUser = UserFactory.Create(Guid.NewGuid(), "Internal", "Internal Last", "12345",
                "internal123@mail.com");
            var aspnetSharedUser = UserFactory.Create(Guid.NewGuid(), "External", "Shared", "12345",
                "external123@external.com");
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

            // Set the INTERNAL user to be the user context.
            A.CallTo(() => userContext.UserId).Returns(Guid.Parse(internalUser.UserId));

            var authorization = new NotificationApplicationAuthorization(context, userContext);

            // There's no assertion for 'does not throw exception' so just executing it as normal.
            await authorization.EnsureAccessAsync(notification.Id);

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
        public async Task NotificationOwnerAccessDoesNotThrowException()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);
            var aspnetInternalUser = UserFactory.Create(Guid.NewGuid(), "Internal", "Internal Last", "12345",
                "internal123@mail.com");
            var aspnetSharedUser = UserFactory.Create(Guid.NewGuid(), "External", "Shared", "12345",
                "external123@external.com");
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

            // Set the Notification owner to be the user context.
            A.CallTo(() => userContext.UserId).Returns(notification.UserId);

            var authorization = new NotificationApplicationAuthorization(context, userContext);

            // There's no assertion for 'does not throw exception' so just executing it as normal.
            await authorization.EnsureAccessAsync(notification.Id);

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
    }
}
