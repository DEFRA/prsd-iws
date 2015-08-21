namespace EA.Iws.Web.Tests.Unit.Controllers.MovementDocument
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.MovementDocument.Controllers;
    using FakeItEasy;
    using Requests.Movement;
    using Xunit;

    public class HomeControllerTests
    {
        private readonly IIwsClient client;
        private readonly HomeController controller;

        public HomeControllerTests()
        {
            client = A.Fake<IIwsClient>();
            controller = new HomeController(() => client);
        }

        [Fact]
        public async Task Create_SendsRequest()
        {
            await controller.Create(Guid.Empty);

            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<CreateMovementForNotificationById>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Create_RedirectsToActualDate()
        {
            var result = await controller.Create(Guid.Empty) as RedirectToRouteResult;

            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("ShipmentDate", result.RouteValues["controller"]);
        }

        [Fact]
        public async Task Create_SendsRequestWithCorrectId()
        {
            await controller.Create(Guid.Empty);

            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<CreateMovementForNotificationById>.That.Matches(r => r.Id == Guid.Empty))).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}