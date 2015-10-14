namespace EA.Iws.Web.Tests.Unit.Controllers.Movement
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.Movement.Controllers;
    using Areas.Movement.ViewModels;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;
    using Xunit;

    public class DateReceivedControllerTests
    {
        private readonly IMediator mediator;
        private readonly DateReceivedController controller;

        private static readonly Guid MovementId = new Guid("09CF4780-D5CB-43FC-98BC-74DD9273896E");
        private static readonly DateTime DateReceived = new DateTime(2015, 9, 1);

        public DateReceivedControllerTests()
        {
            mediator = A.Fake<IMediator>();

            controller = new DateReceivedController(mediator);
        }

        [Fact]
        public async Task GetSendsCorrectRequest()
        {
            await controller.Index(MovementId);

            A.CallTo(() =>
                mediator.SendAsync(A<GetMovementReceiptDateByMovementId>
                    .That.Matches(r => r.MovementId == MovementId)))
                    .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Post_InvalidModel_ReturnsView()
        {
            controller.ModelState.AddModelError("Test", "Error");

            var result = await controller.Index(MovementId, new DateReceivedViewModel());

            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;

            Assert.IsType<DateReceivedViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task PostSendsCorrectRequest()
        {
            var viewModel = new DateReceivedViewModel
            {
                Day = DateReceived.Day,
                Month = DateReceived.Month,
                Year = DateReceived.Year
            };

            await controller.Index(MovementId, viewModel);

            A.CallTo(() =>
                mediator.SendAsync(A<CreateMovementReceiptForMovement>
                        .That.Matches(r => r.MovementId == MovementId && r.DateReceived == DateReceived)))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task PostRedirectsToCorrectScreen()
        {
            var viewModel = new DateReceivedViewModel
            {
                Day = DateReceived.Day,
                Month = DateReceived.Month,
                Year = DateReceived.Year
            };

            var result = await controller.Index(MovementId, viewModel);

            Assert.IsType<RedirectToRouteResult>(result);

            var redirectResult = result as RedirectToRouteResult;

            RouteAssert.RoutesTo(redirectResult.RouteValues, "Index", "Acceptance");
        }
    }
}
