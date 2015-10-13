namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationMovements
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.NotificationMovements.Controllers;
    using Areas.NotificationMovements.ViewModels.CancelMovement;
    using Core.Movement;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Xunit;

    public class CancelMovementControllerTests
    {
        private readonly IMediator mediator;
        private readonly CancelMovementController controller;

        public CancelMovementControllerTests()
        {
            mediator = A.Fake<IMediator>();
            controller = new CancelMovementController(mediator);
        }

        [Fact]
        public async Task Index_SendsRequest()
        {
            await controller.Index(Guid.Empty);

            A.CallTo(() => mediator.SendAsync(A<GetSubmittedMovements>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Create_SendsRequestWithNotificationId()
        {
            await controller.Index(Guid.Empty);

            A.CallTo(() => mediator.SendAsync(A<GetSubmittedMovements>.That.Matches(r => r.NotificationId == Guid.Empty))).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SendsRequestWithNotificationIdAndSelectedMovements()
        {
            CancelMovementsViewModel model = new CancelMovementsViewModel();
            model.NotificationId = Guid.Empty;
            model.SubmittedMovements = new List<SubmittedMovement>
            {
                new SubmittedMovement{ IsSelected = true, MovementId = Guid.Empty, Number = 1},
                new SubmittedMovement{ IsSelected = false, MovementId = Guid.Empty, Number = 2},
                new SubmittedMovement{ IsSelected = true, MovementId = Guid.Empty, Number = 3}
            };

            var result = await controller.Index(model) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("Confirm", result.RouteValues["action"]);
            Assert.Equal("CancelMovement", result.RouteValues["controller"]);
            Assert.Equal(model.NotificationId, result.RouteValues["notificationId"]);
        }

        [Fact]
        public async Task WithoutSelectingMovements_NotValid()
        {
            CancelMovementsViewModel model = new CancelMovementsViewModel();
            model.NotificationId = Guid.Empty;
            model.SubmittedMovements = new List<SubmittedMovement>
            {
                new SubmittedMovement{ IsSelected = false, Number = 1},
                new SubmittedMovement{ IsSelected = false, Number = 2},
                new SubmittedMovement{ IsSelected = false, Number = 3}
            };

            controller.ModelState.AddModelError("test", "test");
            var result = await controller.Index(model) as ViewResult;
            Assert.False(controller.ModelState.IsValid);
            Assert.Equal(string.Empty, result.ViewName);
        }
    }
}