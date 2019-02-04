namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationMovements
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Areas.NotificationMovements.Controllers;
    using Areas.NotificationMovements.ViewModels.PrenotificationBulkUpload;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Web.Infrastructure;
    using Web.Infrastructure.BulkUpload;
    using Xunit;

    public class ReceiptRecoverBulkUploadControllerTests
    {
        private readonly ReceiptRecoveryBulkUploadController controller;
        private readonly IMediator mediator;

        public ReceiptRecoverBulkUploadControllerTests()
        {
            mediator = A.Fake<IMediator>();
            var validator = A.Fake<IBulkMovementValidator>();
            var fileReader = A.Fake<IFileReader>();

            controller = new ReceiptRecoveryBulkUploadController(this.mediator, validator, fileReader);

            var request = A.Fake<HttpRequestBase>();
            var context = A.Fake<HttpContextBase>();

            A.CallTo(() => request.Url).Returns(new Uri("https://test.com"));
            A.CallTo(() => context.Request).Returns(request);

            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);
        }

        [Fact]
        public void GetIndex_ReturnsView()
        {
            var result = controller.Index(Guid.NewGuid()) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ViewName);
        }

        [Fact]
        public void PostIndex_RedirectsToUpload()
        {
            var model = new ReceiptRecoveryBulkUploadViewModel(Guid.NewGuid());

            var result = controller.Index(model.NotificationId, model) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("Upload", (string)result.RouteValues["action"]);
        }

        [Fact]
        public void GetUpload_ReturnsView()
        {
            var model = new ReceiptRecoveryBulkUploadViewModel(Guid.NewGuid());
            var result = controller.Upload(Guid.NewGuid(), model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void PostUpload_MissingFile_ReturnsView()
        {
            controller.ModelState.AddModelError("File", "Missing file");

            var model = new ReceiptRecoveryBulkUploadViewModel(Guid.NewGuid());

            var result = controller.Upload(model.NotificationId, model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.ViewName);
        }
    }
}
