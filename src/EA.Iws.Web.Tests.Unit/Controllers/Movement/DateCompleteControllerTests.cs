namespace EA.Iws.Web.Tests.Unit.Controllers.Movement
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.Movement.Controllers;
    using Core.Shared;
    using FakeItEasy;
    using Requests.MovementOperationReceipt;
    using Web.Areas.Movement.ViewModels;
    using Xunit;

    public class DateCompleteControllerTests
    {
        private readonly IIwsClient client;
        private readonly DateCompleteController controller;

        private static readonly Guid AnyGuid = new Guid("F4034567-92BA-4FB5-A2F8-69CF8304525B");
        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);

        public DateCompleteControllerTests()
        {
            client = A.Fake<IIwsClient>();

            controller = new DateCompleteController(() => client);
        }

        [Fact]
        public async Task GetSendsCorrectRequest()
        {
            await controller.Index(AnyGuid);

            A.CallTo(() => 
                client.SendAsync(
                    A<string>.Ignored, 
                    A<GetMovementOperationReceiptDataByMovementId>
                        .That.Matches(r => r.Id == AnyGuid)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Post_InvalidModel_ReturnsView()
        {
            controller.ModelState.AddModelError("Test", "Error");

            var result = await controller.Index(AnyGuid, new DateCompleteViewModel(null, NotificationType.Recovery));

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<DateCompleteViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task PostSendsCorrectRequest()
        {
            var viewModel = new DateCompleteViewModel(AnyDate, NotificationType.Recovery);

            await controller.Index(AnyGuid, viewModel);

            A.CallTo(() =>
                client.SendAsync(
                    A<string>.Ignored,
                    A<CreateMovementOperationReceiptForMovement>
                        .That.Matches(r =>
                            r.MovementId == AnyGuid
                            && r.DateComplete == AnyDate)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task PostRedirectsToCorrectScreen()
        {
            var result = await PostValidModel(NotificationType.Recovery);

            RouteAssert.RoutesTo(result.RouteValues, "Index", "OperationComplete");
        }

        private async Task<RedirectToRouteResult> PostValidModel(NotificationType notificationType)
        {
            var viewModel = new DateCompleteViewModel(AnyDate, notificationType);

            var result = await controller.Index(AnyGuid, viewModel);

            var redirectResult = Assert.IsType<RedirectToRouteResult>(result);
            
            return redirectResult;
        }
    }
}
