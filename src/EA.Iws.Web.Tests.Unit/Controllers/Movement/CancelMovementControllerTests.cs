namespace EA.Iws.Web.Tests.Unit.Controllers.Movement
{
    using System;
    using System.Threading.Tasks;
    using Areas.NotificationMovements.Controllers;
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
    }
}