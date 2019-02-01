namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationMovements
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Areas.NotificationMovements.Controllers;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Web.Infrastructure;
    using Web.Infrastructure.BulkUpload;
    using Xunit;

    public class ReceiptRecoverBulkUploadControllerTests
    {
        private readonly ReceiptRecoveryBulkUploadController controller;
        private readonly IMediator mediator;

        public ReceiptRecoverBulkUploadControllerTests()
        {
            mediator = A.Fake<IMediator>();
            var validator = A.Fake<IBulkMovementValidator>();
            var fileReader = A.Fake<IFileReader>();

            controller = new ReceiptRecoveryBulkUploadController(this.mediator, validator, fileReader);

            var request = A.Fake<HttpRequestBase>();
            var context = A.Fake<HttpContextBase>();

            A.CallTo(() => request.Url).Returns(new Uri("https://test.com"));
            A.CallTo(() => context.Request).Returns(request);

            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);
        }

        [Fact]
        public void GetIndex_ReturnsView()
        {
            var result = controller.Index(Guid.NewGuid()) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ViewName);
        }
    }
}
