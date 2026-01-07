namespace EA.Iws.Web.Tests.Unit.Controllers.DeleteExportNotification
{
    using EA.Iws.Web.Controllers;
    using EA.Iws.Web.Tests.Unit.TestHelpers;
    using EA.Iws.Web.ViewModels.DeleteExportNotification;
    using EA.Prsd.Core.Mediator;
    using FakeItEasy;
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Xunit;

    public class DeleteExportNotificationControllerTests
    {
        private readonly IMediator mediator;
        private readonly HttpContextBase context;
        private readonly DeleteExportNotificationController controller;

        public DeleteExportNotificationControllerTests()
        {
            mediator = A.Fake<IMediator>();
            controller = new DeleteExportNotificationController(mediator);
            context = A.Fake<HttpContextBase>();
            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);
        }

        [Fact]
        public void ValidModel_ReturnsView()
        {
            var result = controller.Index() as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task InValidModel_ReturnsView()
        {
            var model = new IndexViewModel() { NotificationNumber = string.Empty };
            controller.ModelState.AddModelError("NotificationNumber", "Notification Number required!");
            var result = await controller.Index(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void Check_ReturnsView()
        {
            var model = new DeleteViewModel() { NotificationNumber = "Test1234" };
            var result = controller.Check(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Delete_ReturnsView()
        {
            var model = new DeleteViewModel() { NotificationNumber = "Test1234", NotificationId = Guid.NewGuid(), Success = true };
            var result = await controller.Delete(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("Confirm", result.ViewName);
        }
    }
}
