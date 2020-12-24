namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.RequestHandlers.NotificationAssessment;
    using EA.Iws.Requests.NotificationAssessment;
    using FakeItEasy;
    using Xunit;

    public class SetOperationCodesHandlerTests
    {
        private readonly TestIwsContext context;
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly SetOperationCodesHandler handler;
        private readonly SetOperationCodes message;

        public SetOperationCodesHandlerTests()
        {
            this.context = new TestIwsContext();
            this.notificationRepository = A.Fake<INotificationApplicationRepository>();

            A.CallTo(() => notificationRepository.GetById(A<Guid>.Ignored)).Returns(GetFakeApplication());
           
            this.message = A.Fake<SetOperationCodes>();
            this.handler = new SetOperationCodesHandler(this.context, this.notificationRepository);
        }

        private NotificationApplication GetFakeApplication()
        {
            return A.Fake<NotificationApplication>();
        }

        [Fact]
        public async Task SetOperationCodes()
        {
            var result = await handler.HandleAsync(this.message);

            Assert.True(result);
            A.CallTo(() => this.notificationRepository.GetById(A<Guid>.Ignored))
                .MustHaveHappened();
        }
    }
}
