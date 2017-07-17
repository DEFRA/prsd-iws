namespace EA.Iws.Web.Tests.Unit.Controllers.AdminExportNotificationMovements
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.AdminExportNotificationMovements.Controllers;
    using Areas.AdminExportNotificationMovements.ViewModels.CaptureMovement;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Capture;
    using Web.ViewModels.Shared;
    using Xunit;

    public class CaptureMovementControllerTests
    {
        private readonly CaptureMovementController controller;
        private readonly SearchViewModel model = new SearchViewModel
        {
            Number = 5
        };
        private readonly IMediator mediator;
        private static readonly Guid NotificationId = new Guid("70C125C6-3B69-4B96-84E9-CE84E78C1BB4");
        private static readonly CreateViewModel Model = new CreateViewModel
        {
            Number = 9,
            ActualShipmentDate = new OptionalDateInputViewModel(new DateTime(2015, 1, 1))
        };

        public CaptureMovementControllerTests()
        {
            mediator = A.Fake<IMediator>();
            controller = new CaptureMovementController(mediator);
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

            RouteAssert.RoutesTo(result.RouteValues, "Index", "InternalCapture", "AdminExportMovement");
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
