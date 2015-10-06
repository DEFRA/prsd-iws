namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using System;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core.Mapper;
    using RequestHandlers.Notification;
    using Requests.Notification;
    using Requests.Notification.Overview;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetNotificationInfoInternalHandlerTests
    {
        private static readonly Guid UserId = new Guid("28F41771-44EF-45AC-A6D1-7A153E9B9F4C");
        private static readonly Guid NotificationId = new Guid("64D2FC99-332E-4BCF-8D6A-B9B666723AF2");

        private readonly GetNotificationInfoInternalHandler handler;
        private readonly TestIwsContext context;
        private readonly TestMap map;

        public GetNotificationInfoInternalHandlerTests()
        {
            map = new TestMap();
            var userContext = new TestUserContext(UserId);

            context = new TestIwsContext(userContext);
            handler = A.Fake<GetNotificationInfoInternalHandler>();

            context.NotificationApplications.Add(new TestableNotificationApplication
            {
                UserId = new Guid("C6EC2814-3DF8-4391-B503-0B40614E323C"),
                Id = NotificationId
            });

            context.InternalUsers.Add(new TestableInternalUser
            {
                UserId = UserId.ToString()
            });
        }

        [Fact(Skip = "Need to fix these tests...")]
        public async Task NotificationDoesNotExistThrows()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.HandleAsync(new GetNotificationInfoInternal(Guid.Empty)));
        }

        [Fact(Skip = "Need to fix these tests...")]
        public async Task CallsTheMapper()
        {
            await handler.HandleAsync(new GetNotificationInfoInternal(NotificationId));

            Assert.True(map.MapCalled);
        }

        [Fact(Skip = "Need to fix these tests...")]
        public async Task CallsMapForTheCorrectObject()
        {
            await handler.HandleAsync(new GetNotificationInfoInternal(NotificationId));

            Assert.Equal(NotificationId, map.ObjectMapRequestedFor.Id);
        }

        [Fact(Skip = "Need to fix these tests...")]
        public async Task DoesNotCallSaveChanges()
        {
            await handler.HandleAsync(new GetNotificationInfoInternal(NotificationId));

            Assert.Equal(0, context.SaveChangesCount);
        }

        [Fact(Skip = "Need to fix these tests...")]
        public async Task ReturnsNotificationInfo()
        {
            map.NotificationInfo = new NotificationOverview
            {
                NotificationId = NotificationId
            };

            var result = await handler.HandleAsync(new GetNotificationInfoInternal(NotificationId));

            Assert.Equal(NotificationId, result.NotificationId);
        }

        private class TestMap : IMap<NotificationApplication, NotificationOverview>
        {
            public NotificationOverview NotificationInfo { get; set; }

            public bool MapCalled { get; set; }

            public NotificationApplication ObjectMapRequestedFor { get; set; }

            public NotificationOverview Map(NotificationApplication source)
            {
                MapCalled = true;
                ObjectMapRequestedFor = source;
                return NotificationInfo;
            }
        }
    }
}
