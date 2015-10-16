namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.NotificationMovements.Controllers;
    using Areas.NotificationMovements.ViewModels.ReceiveMovement;
    using Core.Movement;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Web.Controllers;
    using Web.ViewModels.Movement;
    using Web.ViewModels.Shared;
    using Xunit;

    public class NotificationMovementControllerTests
    {
        private static readonly Guid AnyGuid = new Guid("850D92A7-4143-4146-90BB-6D5EF943C790");
        private readonly ReceiveMovementController controller;
        private readonly IMediator mediator;

        public NotificationMovementControllerTests()
        {
            mediator = A.Fake<IMediator>();
            controller = new ReceiveMovementController(mediator);
            A.CallTo(
                () =>
                    mediator.SendAsync(A<GetSubmittedMovementsByNotificationId>.Ignored))
                .Returns(new List<MovementData>());
        }

        [Fact]
        public async Task Get_Receipt_SendsCorrectRequest()
        {
            await controller.Index(AnyGuid);

            A.CallTo(
                () =>
                    mediator.SendAsync(
                        A<GetSubmittedMovementsByNotificationId>.That.Matches(r => r.Id == AnyGuid)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Get_Receipt_ReturnsCorrectViewModel()
        {
            var result = await controller.Index(AnyGuid) as ViewResult;

            Assert.IsType<MovementReceiptViewModel>(result.Model);
            Assert.Equal(AnyGuid, (result.Model as MovementReceiptViewModel).NotificationId);
        }

        [Fact]
        public async Task Get_Index_ListsMovementsForNotification()
        {
            var movements = new Dictionary<int, Guid>();
            movements.Add(1, AnyGuid);

            A.CallTo(() => mediator.SendAsync(A<GetMovementsForNotificationById>.Ignored)).Returns(movements);
            
            var c = new NotificationMovementController(mediator);
            var result = await c.Index(AnyGuid);

            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;

            Assert.IsType<MovementsViewModel>(viewResult.Model);
            var model = viewResult.Model as MovementsViewModel;

            Assert.NotEmpty(model.Movements);
        }
        
        [Fact]
        public void Post_ReceiptWithModelErrors_ReturnsView()
        {
            controller.ModelState.AddModelError("a", "b");

            var result =
                controller.Index(AnyGuid, new MovementReceiptViewModel(AnyGuid, new List<MovementData>())) as
                    ViewResult;

            Assert.IsType<MovementReceiptViewModel>(result.Model);
            Assert.Equal(AnyGuid, (result.Model as MovementReceiptViewModel).NotificationId);
        }

        [Fact]
        public void Post_Receipt_RedirectsToAction()
        {
            var movementId = new Guid("E3CB71D2-7D9A-4C1D-A30B-0EAACFCE8545");

            var result = controller.Index(AnyGuid, new MovementReceiptViewModel
            {
                NotificationId = AnyGuid,
                RadioButtons = new StringGuidRadioButtons
                {
                    SelectedValue = movementId
                }
            }) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("DateReceived", result.RouteValues["controller"]);
            Assert.Equal("Movement", result.RouteValues["area"]);
            Assert.Equal(movementId, result.RouteValues["id"]);
        }
    }
}