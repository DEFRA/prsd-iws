namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using System;
    using System.Threading.Tasks;
    using FakeItEasy;
    using Prsd.Core.Mapper;
    using RequestHandlers.Notification;
    using Requests.Notification;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetNotificationInfoInternalHandlerTests
    {
        private static readonly Guid UserId = new Guid("28F41771-44EF-45AC-A6D1-7A153E9B9F4C");
        private static readonly Guid NotificationId = new Guid("64D2FC99-332E-4BCF-8D6A-B9B666723AF2");

        private readonly GetNotificationOverviewInternalHandler handler;
        private readonly TestIwsContext context;
        private readonly IMapper mapper;

        public GetNotificationInfoInternalHandlerTests()
        {
            var userContext = new TestUserContext(UserId);
            mapper = A.Fake<IMapper>();
            context = new TestIwsContext(userContext);
            handler = new GetNotificationOverviewInternalHandler(context, userContext, null, mapper);

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

        [Fact]
        public async Task NotificationDoesNotExistThrows()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.HandleAsync(new GetNotificationOverviewInternal(Guid.Empty)));
        }
        
        [Fact(Skip = "Waiting for an 'overview' repository")]
        public async Task DoesNotCallSaveChanges()
        {
            await handler.HandleAsync(new GetNotificationOverviewInternal(NotificationId));

            Assert.Equal(0, context.SaveChangesCount);
        }

        [Fact(Skip = "Waiting for an 'overview' repository")]
        public async Task ReturnsNotificationInfo()
        {
            var result = await handler.HandleAsync(new GetNotificationOverviewInternal(NotificationId));

            Assert.Equal(NotificationId, result.NotificationId);
        }
    }
}
