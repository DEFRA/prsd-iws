namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.ShareNotification;
    using Core.Documents;
    using Core.Notification;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.SharedUsers;
    using Requests.Users;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using TestHelpers;
    using Xunit;
    public class HomeControllerTests
    {
        private readonly IMediator mediator;
        private readonly HomeController homeController;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");
        private readonly FileData fileData = new FileData("test.pdf", FileType.Pdf, new byte[0]);

        public HomeControllerTests()
        {
            mediator = A.Fake<IMediator>();
            homeController = new HomeController(mediator);
        }

        [Fact]
        public async Task GenerateNotificationPreviewDocument_ReturnsFile()
        {
            A.CallTo(
                () =>
                mediator.SendAsync(A<GenerateNotificationDocument>.That.Matches(p => p.NotificationId == notificationId)))
                .Returns(fileData);

            var result = await homeController.GenerateNotificationPreviewDocument(notificationId) as FileContentResult;

            Assert.Equal(result.FileDownloadName, ".pdf");
        }
    }
}
