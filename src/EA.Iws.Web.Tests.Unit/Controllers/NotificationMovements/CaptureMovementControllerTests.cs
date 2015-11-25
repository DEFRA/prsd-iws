namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationMovements
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.NotificationMovements.Controllers;
    using Areas.NotificationMovements.ViewModels.CaptureMovement;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Capture;
    using Web.ViewModels.Shared;
    using Xunit;

    public class CaptureMovementControllerTests
    {
        private readonly CaptureMovementController controller;
        private readonly SearchViewModel model = new SearchViewModel();
        private readonly IMediator mediator;
        private static readonly Guid NotificationId = new Guid("70C125C6-3B69-4B96-84E9-CE84E78C1BB4");
        private static readonly CreateViewModel Model = new CreateViewModel
        {
            ActualShipmentDate = new OptionalDateInputViewModel(new DateTime(2015, 1, 1))
        };

        public CaptureMovementControllerTests()
        {
            mediator = A.Fake<IMediator>();
            controller = new CaptureMovementController(mediator);
        }

        [Theory]
        [InlineData("533AF00E-95D2-4D74-9A21-EB2752385B69", 7)]
        [InlineData("49EF1E9E-ED8F-41AA-9E80-9D80C40D517C", 5)]
        public async Task Get_PopulatesNextAvailableShipmentNumber(string id, int number)
        {
            var notificationId = new Guid(id);

            A.CallTo(
                () =>
                    mediator.SendAsync(
                        A<GetNextAvailableMovementNumberForNotification>.That.Matches(r => r.Id == notificationId))).Returns(number);

            var result = await controller.Index(notificationId) as ViewResult;
            
            var viewModel = Assert.IsType<SearchViewModel>(result.Model);

            Assert.Equal(number, viewModel.Number);
        }

        [Fact]
        public async Task Post_RedirectsToMovementCapturePage_OnMovementDoesnotExist()
        {
            var notificationId = new Guid("18264B29-00F8-46A1-9640-708CE4F0ADD6");

            A.CallTo(
                () =>
                    mediator.SendAsync(
                        A<GetMovementIdIfExists>.That.Matches(r => r.NotificationId == notificationId))).Returns<Guid?>(null);

            var result = await controller.Index(notificationId, model) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "Create", null);
        }

        [Fact]
        public async Task Post_WhenMovementExists_ReturnsError()
        {
            var notificationId = new Guid("18264B29-00F8-46A1-9640-708CE4F0ADD6");

            A.CallTo(
                () =>
                    mediator.SendAsync(
                        A<GetMovementIdIfExists>.That.Matches(r => r.NotificationId == notificationId))).Returns(new Guid("BDE8DF37-4D4B-41C7-8692-357C30C647F6"));

            var result = await controller.Index(notificationId, model) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "Index", "InternalCapture", "Movement");
        }

        [Fact]
        public void Create_Get_RetrievesMovementNumber()
        {
            controller.TempData["MovementNumberKey"] = 52;

            var result = controller.Create(NotificationId) as ViewResult;

            var viewModel = Assert.IsType<CreateViewModel>(result.Model);

            Assert.Equal(52, viewModel.Number);
        }

        [Fact]
        public void Create_Get_CantRetrieveNumber_RedirectsToSearch()
        {
            var result = controller.Create(NotificationId) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "Index", null);
        }

        [Fact]
        public async Task Create_Post_Fail_ReturnsView()
        {
            A.CallTo(() => mediator.SendAsync(A<CreateMovementInternal>.Ignored)).Returns(false);

            var result = await controller.Create(NotificationId, Model) as RedirectToRouteResult;

            Assert.False(controller.ModelState.IsValid);
        }
    }
}
