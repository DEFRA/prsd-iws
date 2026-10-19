namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationAssessment.Payment
{
    using Domain.Security;
    using EA.Iws.Domain.NotificationAssessment;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using RequestHandlers.NotificationAssessment.Payment;
    using Requests.NotificationAssessment.Payment;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class SaveGovUkPaySessionHandlerTests
    {
        private readonly SaveGovUkPaySessionHandler handler;
        private readonly INotificationApplicationAuthorization authorization;
        private readonly IGovUkPaySessionRepository paySessionRepository;
        private readonly TestIwsContext context;
        private readonly IUserContext userContext;

        private readonly Guid notificationId = Guid.NewGuid();
        private readonly Guid userId = Guid.NewGuid();

        public SaveGovUkPaySessionHandlerTests()
        {
            authorization = A.Fake<INotificationApplicationAuthorization>();
            paySessionRepository = A.Fake<IGovUkPaySessionRepository>();
            context = new TestIwsContext();
            userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(userId);

            handler = new SaveGovUkPaySessionHandler(authorization, paySessionRepository, context, userContext);
        }

        private SaveGovUkPaySession GetRequest()
        {
            return new SaveGovUkPaySession(notificationId, "payment-id", "GB1234567", Guid.NewGuid().ToString("N"), 125.50m);
        }

        [Fact]
        public async Task HandleAsync_EnsuresAccess()
        {
            await handler.HandleAsync(GetRequest());

            A.CallTo(() => authorization.EnsureAccessAsync(notificationId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_AddsSessionToRepository()
        {
            await handler.HandleAsync(GetRequest());

            A.CallTo(() => paySessionRepository.Add(A<Domain.NotificationAssessment.GovUkPaySession>.That.Matches(
                s => s.NotificationId == notificationId && s.UserId == userId))).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_SavesChanges()
        {
            await handler.HandleAsync(GetRequest());

            Assert.Equal(1, context.SaveChangesCount);
        }

        [Fact]
        public async Task HandleAsync_ReturnsTrue()
        {
            var result = await handler.HandleAsync(GetRequest());

            Assert.True(result);
        }
    }
}