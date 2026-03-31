namespace EA.Iws.RequestHandlers.Tests.Unit.DeleteExportNotification
{
    using EA.Iws.Core.Notification;
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.RequestHandlers.Notification;
    using EA.Iws.Requests.Notification;
    using FakeItEasy;
    using System.Threading.Tasks;
    using Xunit;

    public class GetExportNotificationIdHandlerTests
    {
        private readonly INotificationApplicationRepository repository;
        private readonly GetExportNotificationIdHandler handler;
        private readonly GetExportNotificationId getExportNotificationId;

        public GetExportNotificationIdHandlerTests()
        {
            this.repository = A.Fake<INotificationApplicationRepository>();
            this.getExportNotificationId = A.Fake<GetExportNotificationId>();
            this.handler = new GetExportNotificationIdHandler(this.repository);
        }

        [Fact]
        public async Task GetExportNotification_Returns_Success()
        {
            A.CallTo(() => repository.ValidateExportNotification(A<string>.Ignored)).Returns(new DeleteExportNotificationDetails()
            {
                ErrorMessage = string.Empty,
                IsNotificationCanDeleted = true
            });
            var result = await handler.HandleAsync(this.getExportNotificationId);

            Assert.NotNull(result);
            Assert.Empty(result.ErrorMessage);
            Assert.True(result.IsNotificationCanDeleted);
        }

        [Fact]
        public async Task GetExportNotification_Returns_NoNotificationFound_Error()
        {
            A.CallTo(() => repository.ValidateExportNotification(A<string>.Ignored)).Returns(new DeleteExportNotificationDetails()
            {
                ErrorMessage = "There is no notification with this notification number",
                IsNotificationCanDeleted = false
            });
            var result = await handler.HandleAsync(this.getExportNotificationId);

            Assert.NotNull(result);
            Assert.Equal("There is no notification with this notification number", result.ErrorMessage);
            Assert.False(result.IsNotificationCanDeleted);
        }

        [Fact]
        public async Task GetExportNotification_Returns_DoNotHavePermission_Error()
        {
            A.CallTo(() => repository.ValidateExportNotification(A<string>.Ignored)).Returns(new DeleteExportNotificationDetails()
            {
                ErrorMessage = "You do not have permission to delete this notification",
                IsNotificationCanDeleted = false
            });
            var result = await handler.HandleAsync(this.getExportNotificationId);

            Assert.NotNull(result);
            Assert.Equal("You do not have permission to delete this notification", result.ErrorMessage);
            Assert.False(result.IsNotificationCanDeleted);
        }

        [Fact]
        public async Task GetExportNotification_Returns_DoNotHavePermission_ToDeleteSubmittedNotification_Error()
        {
            A.CallTo(() => repository.ValidateExportNotification(A<string>.Ignored)).Returns(new DeleteExportNotificationDetails()
            {
                ErrorMessage = "You do not have permission to delete the notification once it has been Submitted",
                IsNotificationCanDeleted = false
            });
            var result = await handler.HandleAsync(this.getExportNotificationId);

            Assert.NotNull(result);
            Assert.Equal("You do not have permission to delete the notification once it has been Submitted", result.ErrorMessage);
            Assert.False(result.IsNotificationCanDeleted);
        }
    }
}
