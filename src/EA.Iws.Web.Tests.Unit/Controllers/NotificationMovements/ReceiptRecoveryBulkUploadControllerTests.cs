﻿namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationMovements
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Areas.NotificationMovements.Controllers;
    using Areas.NotificationMovements.ViewModels.ReceiptRecoveryBulkUpload;
    using Core.Shared;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Web.Infrastructure;
    using Web.Infrastructure.BulkReceiptRecovery;
    using Web.ViewModels.Shared;
    using Xunit;

    public class ReceiptRecoverBulkUploadControllerTests
    {
        private readonly ReceiptRecoveryBulkUploadController controller;
        private readonly IMediator mediator;
        private NotificationType type = NotificationType.Recovery;

        public ReceiptRecoverBulkUploadControllerTests()
        {
            mediator = A.Fake<IMediator>();
            var validator = A.Fake<IReceiptRecoveryValidator>();
            var fileReader = A.Fake<IFileReader>();

            controller = new ReceiptRecoveryBulkUploadController(this.mediator, validator, fileReader);

            var request = A.Fake<HttpRequestBase>();
            var context = A.Fake<HttpContextBase>();

            A.CallTo(() => request.Url).Returns(new Uri("https://test.com"));
            A.CallTo(() => context.Request).Returns(request);
            A.CallTo(() => mediator.SendAsync(A<GetNotificationBasicInfo>._)).Returns(A.Fake<NotificationBasicInfo>());

            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);
        }

        [Fact]
        public async Task GetIndex_ReturnsView()
        {
            var result = await controller.Index(Guid.NewGuid()) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ViewName);
        }

        [Fact]
        public void PostIndex_RedirectsToUpload()
        {
            var model = new ReceiptRecoveryBulkUploadViewModel(Guid.NewGuid(), type);

            var result = controller.Index(model.NotificationId, model) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("Upload", (string)result.RouteValues["action"]);
        }

        [Fact]
        public async Task GetUpload_ReturnsView()
        {
            var model = new ReceiptRecoveryBulkUploadViewModel(Guid.NewGuid(), type);
            model.File = A.Fake<HttpPostedFileBase>();
            var result = await controller.Upload(Guid.NewGuid(), model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("Documents", result.ViewName);
        }

        [Fact]
        public async Task PostUpload_MissingFile_ReturnsView()
        {
            controller.ModelState.AddModelError("File", "Missing file");

            var model = new ReceiptRecoveryBulkUploadViewModel(Guid.NewGuid(), type);
            var result = await controller.Upload(model.NotificationId, model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task GetWarning_ReturnsView()
        {
            var result = await controller.Warning(Guid.NewGuid()) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void PostWarning_LeaveUpload_RedirectsToOptions()
        {
            var model = new WarningChoiceViewModel(Guid.NewGuid(), type);

            var warningChoice = RadioButtonStringCollectionViewModel.CreateFromEnum(WarningChoicesList.Leave);

            model.WarningChoices = warningChoice;

            var result = controller.Warning(model) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("Index", (string)result.RouteValues["action"]);
            Assert.Equal("Options", (string)result.RouteValues["controller"]);
        }
    }
}
