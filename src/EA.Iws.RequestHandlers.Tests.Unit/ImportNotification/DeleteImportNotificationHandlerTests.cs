namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification
{
    using System;
    using System.Threading.Tasks;
    using EA.Iws.Domain.ImportNotification;
    using EA.Iws.RequestHandlers.ImportNotification;
    using EA.Iws.RequestHandlers.ImportNotificationAssessment;
    using EA.Iws.Requests.ImportNotification;
    using EA.Iws.Requests.ImportNotificationAssessment;
    using FakeItEasy;
    using Xunit;

    public class DeleteImportNotificationHandlerTests
    {
        private readonly IImportNotificationRepository repo;
        private readonly DeleteImportNotificationHandler handler;
        private readonly GetImportNotificationNumberByIdHandler notificationHandler;
        private readonly DeleteImportNotification message;
        private readonly GetImportNotificationNumberById importNotificationId;

        public DeleteImportNotificationHandlerTests()
        {
            this.repo = A.Fake<IImportNotificationRepository>();
            this.message = A.Fake<DeleteImportNotification>();
            this.handler = new DeleteImportNotificationHandler(this.repo);
            this.notificationHandler = new GetImportNotificationNumberByIdHandler(this.repo);
            this.importNotificationId = A.Fake<GetImportNotificationNumberById>();
        }

        [Fact]
        public async Task DeleteImportNotification_ReturnsTrue()
        {
            A.CallTo(() => repo.Delete(A<Guid>.Ignored)).Returns(true);
            var result = await handler.HandleAsync(this.message);
            Assert.True(result);
            A.CallTo(() => this.repo.Delete(A<Guid>.Ignored)).MustHaveHappened();
        }

        [Fact]
        public async Task DeleteImportNotification_NotExist_ReturnsEmpty()
        {
            A.CallTo(() => repo.Delete(this.message.NotificationId)).Returns(true);
            var result = await notificationHandler.HandleAsync(this.importNotificationId);
            Assert.Empty(result);
        }
    }
}
