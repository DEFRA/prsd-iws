namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationMovements
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.NotificationMovements.Controllers;
    using Core.Documents;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.BulkUpload;
    using Web.Infrastructure;
    using Xunit;

    public class BulkUploadTemplateControllerTests
    {
        private readonly BulkUploadTemplateController controller;
        private readonly IMediator mediator;
        private readonly Guid notificationId;

        public BulkUploadTemplateControllerTests()
        {
            notificationId = Guid.NewGuid();
            mediator = A.Fake<IMediator>();
            controller = new BulkUploadTemplateController(mediator);
        }

        [Fact]
        public async Task GetPrenotificationTemplateReturnsExcelFile()
        {
            A.CallTo(() => mediator.SendAsync(new GetBulkUploadTemplate(notificationId, BulkType.Prenotification)))
                .Returns(new byte[100]);
            
            var result = await controller.PrenotificationTemplate(notificationId);

            Assert.IsType<FileContentResult>(result);

            var fileResult = result as FileContentResult;

            Assert.NotNull(fileResult);
            Assert.Equal(MimeTypes.MSExcelXml, fileResult.ContentType);
            Assert.True(fileResult.FileDownloadName.EndsWith(".xlsx"));
        }
    }
}
