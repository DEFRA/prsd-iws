namespace EA.Iws.Web.Tests.Unit.Controllers.Admin
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.Admin.Controllers;
    using Areas.Admin.ViewModels.ImportNotification;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Web.ViewModels.Shared;
    using Xunit;

    public class ImportNotificationControllerTests
    {
        private readonly ImportNotificationController controller;

        public ImportNotificationControllerTests()
        {
            controller = new ImportNotificationController(A.Fake<IMediator>());
        }

        [Fact]
        public void GetIndex_ReturnsNewViewModel()
        {
            var result = controller.Number() as ViewResult;

            var model = result.Model as NotificationNumberViewModel;
            Assert.Null(model);
        }

        [Fact]
        public async Task WithoutNotificationNumber_NotValid()
        {
            NotificationNumberViewModel model = new NotificationNumberViewModel();
            controller.ModelState.AddModelError("test", "test");

            var result = await controller.Number(model) as ViewResult;
            Assert.False(controller.ModelState.IsValid);
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void ReceivedDate_InFuture_NotValid()
        {
            NotificationReceivedDateViewModel model = new NotificationReceivedDateViewModel();
            model.NotificationReceived = new OptionalDateInputViewModel(DateTime.UtcNow.AddDays(1));

            var controller = GetMockAssessmentController(model);

            controller.ReceivedDate(model);

            Assert.True(controller.ModelState.ContainsKey("NotificationReceived"));
        }

        [Fact]
        public void ReceivedDate_Today_Valid()
        {
            NotificationReceivedDateViewModel model = new NotificationReceivedDateViewModel();
            model.NotificationReceived = new OptionalDateInputViewModel(DateTime.UtcNow);

            var controller = GetMockAssessmentController(model);

            controller.ReceivedDate(model);

            Assert.True(controller.ModelState.IsValid);
        }

        [Fact]
        public void ReceivedDate_InPast_Valid()
        {
            NotificationReceivedDateViewModel model = new NotificationReceivedDateViewModel();
            model.NotificationReceived = new OptionalDateInputViewModel(DateTime.UtcNow.AddDays(-1));

            var controller = GetMockAssessmentController(model);

            controller.ReceivedDate(model);

            Assert.True(controller.ModelState.IsValid);
        }

        private ImportNotificationController GetMockAssessmentController(object viewModel)
        {
            var assessmentController = new ImportNotificationController(A.Fake<IMediator>());
            // Mimic the behaviour of the model binder which is responsible for Validating the Model
            var validationContext = new ValidationContext(viewModel, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(viewModel, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                assessmentController.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            return assessmentController;
        }
    }
}
