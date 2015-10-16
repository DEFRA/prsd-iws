namespace EA.Iws.Web.Tests.Unit.Controllers.Admin
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.Admin.Controllers;
    using Areas.Admin.ViewModels.ImportNotification;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Xunit;

    public class ImportNotificationControllerTests
    {
        private readonly ImportNotificationController controller;

        public ImportNotificationControllerTests()
        {
            controller = new ImportNotificationController(A.Fake<IMediator>());
        }

        [Fact]
        public void GetIndex_ReturnsNewViewModel()
        {
            var result = controller.Number() as ViewResult;

            var model = result.Model as NotificationNumberViewModel;
            Assert.Null(model);
        }

        [Fact]
        public async Task WithoutNotificationNumber_NotValid()
        {
            NotificationNumberViewModel model = new NotificationNumberViewModel();
            controller.ModelState.AddModelError("test", "test");

            var result = await controller.Number(model) as ViewResult;
            Assert.False(controller.ModelState.IsValid);
            Assert.Equal(string.Empty, result.ViewName);
        }
    }
}
