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
    using Web.Infrastructure.BulkUpload;
    using Web.ViewModels.Shared;
    using Xunit;

    public class PrenotificationBulkUploadControllerTests
    {
        private readonly PrenotificationBulkUploadController controller;

        public PrenotificationBulkUploadControllerTests()
        {
            var mediator = A.Fake<IMediator>();
            var validator = A.Fake<IBulkMovementValidator>();

            controller = new PrenotificationBulkUploadController(mediator, validator);

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
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void PostIndex_RedirectsToUpload()
        {
            var model = new PrenotificationBulkUploadViewModel(Guid.NewGuid());

            var result = controller.Index(model.NotificationId, model) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("UploadPrenotifications", (string)result.RouteValues["action"]);
        }

        [Fact]
        public void GetUploadPrenotifications_ReturnsView()
        {
            var result = controller.Index(Guid.NewGuid()) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task PostUploadPrenotifications_MissingFile_ReturnsView()
        {
            controller.ModelState.AddModelError("File", "Missing file");

            var model = new PrenotificationBulkUploadViewModel(Guid.NewGuid());

            var result = await controller.UploadPrenotifications(model.NotificationId, model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void GetWarning_ReturnsView()
        {
            var result = controller.Warning(Guid.NewGuid()) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void PostWarning_LeaveUpload_RedirectsToOptions()
        {
            var model = new WarningChoiceViewModel(Guid.NewGuid());

            var warningChoice = RadioButtonStringCollectionViewModel.CreateFromEnum(WarningChoicesList.Leave);

            model.WarningChoices = warningChoice;

            var result = controller.Warning(model, string.Empty) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("Index", (string)result.RouteValues["action"]);
            Assert.Equal("Options", (string)result.RouteValues["controller"]);
        }
    }
}
