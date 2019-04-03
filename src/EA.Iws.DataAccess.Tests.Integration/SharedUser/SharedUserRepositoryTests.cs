namespace EA.Iws.DataAccess.Tests.Integration.SharedUser
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.Security;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Repositories;
    using TestHelpers.Helpers;
    using Xunit;
    using NotificationApplicationFactory = TestHelpers.Helpers.NotificationApplicationFactory;

    public class SharedUserRepositoryTests : IDisposable
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization authorization;
        private readonly SharedUserRepository repository;

        private readonly Guid[] preRunNotifications;
        private readonly User ownerUser;
        private readonly User sharedUser;
        private readonly NotificationApplication notification;

        public SharedUserRepositoryTests()
        {
            ownerUser = UserFactory.Create(Guid.NewGuid(), "Owner", "User", "12345",
                "owneruser111@mail.com");
            sharedUser = UserFactory.Create(Guid.NewGuid(), "Shared", "User", "12345",
                "shareduser111@external.com");

            var userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(Guid.Parse(ownerUser.Id));

            context = new IwsContext(userContext, A.Fake<IEventDispatcher>());
            authorization = A.Fake<INotificationApplicationAuthorization>();
            repository = new SharedUserRepository(context, authorization);

            preRunNotifications = context.NotificationApplications.Select(na => na.Id).ToArray();

            notification = NotificationApplicationFactory.Create(Guid.Parse(ownerUser.Id), NotificationType.Recovery,
                UKCompetentAuthority.England, 20191);

            context.Users.Add(ownerUser);
            context.Users.Add(sharedUser);
            context.NotificationApplications.Add(notification);
            context.SaveChanges();
        }

        [Fact]
        public async Task AddSharedUserChecksAuthorization()
        {
            var shared = new SharedUser(notification.Id, sharedUser.Id, DateTimeOffset.Now);

            await repository.AddSharedUser(shared);

            A.CallTo(() => authorization.EnsureAccessIsOwnerAsync(shared.NotificationId)).MustHaveHappened();

            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task RemoveSharedUserChecksAuthorization()
        {
            var shared = new SharedUser(notification.Id, sharedUser.Id, DateTimeOffset.Now);

            context.SharedUser.Add(shared);
            await context.SaveChangesAsync();

            await repository.RemoveSharedUser(notification.Id, shared.Id);

            A.CallTo(() => authorization.EnsureAccessIsOwnerAsync(shared.NotificationId)).MustHaveHappened();

            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllSharedUsersChecksAuthorization()
        {
            var shared = new SharedUser(notification.Id, sharedUser.Id, DateTimeOffset.Now);

            context.SharedUser.Add(shared);
            await context.SaveChangesAsync();

            await repository.GetAllSharedUsers(notification.Id);

            A.CallTo(() => authorization.EnsureAccessAsync(notification.Id)).MustHaveHappened();
        }

        [Fact]
        public async Task GetSharedUserByIdChecksAuthorization()
        {
            var shared = new SharedUser(notification.Id, sharedUser.Id, DateTimeOffset.Now);

            context.SharedUser.Add(shared);
            await context.SaveChangesAsync();

            await repository.GetSharedUserById(notification.Id, shared.Id);

            A.CallTo(() => authorization.EnsureAccessAsync(notification.Id)).MustHaveHappened();
        }

        public void Dispose()
        {
            var createdNotifications =
                context.NotificationApplications.Where(n => !preRunNotifications.Contains(n.Id))
                    .Select(n => n.Id)
                    .ToArray();

            foreach (var createdNotification in createdNotifications)
            {
                DatabaseDataDeleter.DeleteDataForNotification(createdNotification, context);
            }

            context.Entry(ownerUser).State = EntityState.Deleted;
            context.Entry(sharedUser).State = EntityState.Deleted;
            context.SaveChanges();

            context.Dispose();
        }
    }
}
