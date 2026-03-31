namespace EA.Iws.RequestHandlers.Tests.Unit.DeleteExportNotification
{
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.RequestHandlers.DeleteNotification;
    using EA.Iws.Requests.DeleteNotification;
    using FakeItEasy;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class DeleteExportNotificationHandlerTests
    {
        private readonly INotificationApplicationRepository repository;
        private readonly DeleteExportNotificationHandler handler;
        private readonly DeleteExportNotification deleteExportNotification;

        public DeleteExportNotificationHandlerTests()
        {
            this.repository = A.Fake<INotificationApplicationRepository>();
            this.deleteExportNotification = A.Fake<DeleteExportNotification>();
            this.handler = new DeleteExportNotificationHandler(this.repository);
        }

        [Fact]
        public async Task DeleteExportNotification_ReturnsTrue()
        {
            var result = await handler.HandleAsync(deleteExportNotification);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteExportNotification_MustHaveHappenedOnceExactly()
        {
            var result = await handler.HandleAsync(deleteExportNotification);

            Assert.True(result);

            A.CallTo(() => repository.DeleteExportNotification(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}
