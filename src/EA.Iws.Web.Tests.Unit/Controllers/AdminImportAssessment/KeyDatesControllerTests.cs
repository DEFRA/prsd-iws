namespace EA.Iws.Web.Tests.Unit.Controllers.AdminImportAssessment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Areas.AdminImportAssessment.Controllers;
    using Areas.AdminImportAssessment.ViewModels.KeyDates;
    using Core.ImportNotificationAssessment;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;
    using Web.ViewModels.Shared;
    using Xunit;

    public class KeyDatesControllerTests
    {
        private readonly IMediator mediator;
        private readonly Guid notificationId = new Guid("65214A82-EE7B-42FF-A4A1-220B2A7E74BB");
        private readonly DateTime notificationReceivedDate = new DateTime(2015, 8, 1);
        private readonly KeyDatesController controller;
        private readonly DateTime paymentReceivedDate = new DateTime(2015, 8, 2);
        private readonly DateTime commencementDate = new DateTime(2015, 8, 3);
        private readonly DateTime completeDate = new DateTime(2015, 8, 20);
        private readonly DateTime transmittedDate = new DateTime(2015, 8, 21);
        private readonly DateTime acknowledgedDate = new DateTime(2015, 8, 22);
        private readonly DateTime decisionRequiredDate = new DateTime(2015, 8, 22);

        public KeyDatesControllerTests()
        {
            mediator = A.Fake<IMediator>();

            A.CallTo(
                () => mediator.SendAsync(A<GetKeyDates>.That.Matches(p => p.ImportNotificationId == notificationId)))
                .Returns(new KeyDatesData
                {
                    NotificationReceived = notificationReceivedDate,
                    PaymentReceived = paymentReceivedDate,
                    AcknowlegedDate = acknowledgedDate,
                    DecisionRequiredByDate = decisionRequiredDate
                });

            controller = new KeyDatesController(mediator);
        }

        [Fact]
        public async Task Index_FetchesNotificationDates()
        {
            await controller.Index(notificationId, 0);

            A.CallTo(() => mediator.SendAsync(A<GetKeyDates>.That.Matches(dates => dates.ImportNotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task AssessmentCommenced_ValidInput_NoValidationError()
        {
            var model = GetValidViewModel();
            model.NewDate = new OptionalDateInputViewModel(commencementDate);
            model.NameOfOfficer = "Officer";
            model.Command = KeyDatesCommand.BeginAssessment;

            var controller = GetMockAssessmentController(model);

            await controller.Index(notificationId, model);

            Assert.True(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task AssessmentCommenced_InFuture_Invalid()
        {
            await Date_InFuture_Invalid(KeyDatesCommand.BeginAssessment);
        }

        [Fact]
        public async Task AssessmentCommenced_NotificationReceivedDate_MustBePresent()
        {
            await NotificationReceivedDate_MustBePresent(KeyDatesCommand.BeginAssessment);
        }

        [Fact]
        public async Task AssessmentCommenced_NotBefore_NotificationReceivedDate()
        {
            await NotBefore_NotificationReceivedDate(KeyDatesCommand.BeginAssessment);
        }

        [Fact]
        public async Task Complete_InFuture_Invalid()
        {
            await Date_InFuture_Invalid(KeyDatesCommand.NotificationComplete);
        }

        [Fact]
        public async Task Complete_NotificationReceivedDate_MustBePresent()
        {
            await NotificationReceivedDate_MustBePresent(KeyDatesCommand.NotificationComplete);
        }

        [Fact]
        public async Task Complete_PaymentReceivedDate_MustBePresent()
        {
            await PaymentReceivedDate_MustBePresent(KeyDatesCommand.NotificationComplete);
        }

        [Fact]
        public async Task Complete_NotBefore_NotificationReceivedDate()
        {
            await NotBefore_NotificationReceivedDate(KeyDatesCommand.NotificationComplete);
        }

        [Fact]
        public async Task Complete_NotBefore_PaymentReceivedDate()
        {
            await NotBefore_PaymentReceivedDate(KeyDatesCommand.NotificationComplete);
        }

        [Fact]
        public async Task Acknowledged_InFuture_Invalid()
        {
            await Date_InFuture_Invalid(KeyDatesCommand.NotificationAcknowledged);
        }

        [Fact]
        public async Task Acknowledged_NotificationReceivedDate_MustBePresent()
        {
            await NotificationReceivedDate_MustBePresent(KeyDatesCommand.NotificationAcknowledged);
        }

        [Fact]
        public async Task Acknowledged_NotBefore_NotificationReceivedDate()
        {
            await NotBefore_NotificationReceivedDate(KeyDatesCommand.NotificationAcknowledged);
        }

        private async Task NotificationReceivedDate_MustBePresent(KeyDatesCommand command)
        {
            var model = GetValidViewModel();
            model.NotificationCompleteDate = null;
            model.NotificationReceivedDate = null;
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = command;

            var controller = GetMockAssessmentController(model);

            await controller.Index(notificationId, model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        private async Task NotBefore_NotificationReceivedDate(KeyDatesCommand command)
        {
            var model = GetValidViewModel();
            model.NotificationCompleteDate = null;
            model.NewDate = new OptionalDateInputViewModel(new DateTime(2015, 7, 2));
            model.Command = command;

            var controller = GetMockAssessmentController(model);

            await controller.Index(notificationId, model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        private async Task Date_InFuture_Invalid(KeyDatesCommand command)
        {
            var model = GetValidViewModel();
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow.AddDays(1));
            model.Command = command;

            var controller = GetMockAssessmentController(model);

            await controller.Index(notificationId, model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        private async Task NotBefore_PaymentReceivedDate(KeyDatesCommand command)
        {
            var model = GetValidViewModel();
            model.NotificationCompleteDate = null;
            model.NewDate = new OptionalDateInputViewModel(new DateTime(2015, 8, 1));
            model.Command = command;

            var controller = GetMockAssessmentController(model);

            await controller.Index(notificationId, model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        private async Task PaymentReceivedDate_MustBePresent(KeyDatesCommand command)
        {
            var model = GetValidViewModel();
            model.NotificationCompleteDate = null;
            model.NotificationReceivedDate = new OptionalDateInputViewModel(notificationReceivedDate);
            model.PaymentReceivedDate = null;
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = command;

            var controller = GetMockAssessmentController(model);

            await controller.Index(notificationId, model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        private KeyDatesController GetMockAssessmentController(object viewModel)
        {
            var assessmentController = new KeyDatesController(mediator);
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

        private KeyDatesViewModel GetValidViewModel()
        {
            var model = new KeyDatesViewModel();

            model.NotificationId = notificationId;
            model.NotificationReceivedDate = new OptionalDateInputViewModel(notificationReceivedDate);
            model.PaymentReceivedDate = paymentReceivedDate;
            model.AssessmentStartedDate = new OptionalDateInputViewModel(commencementDate);
            model.NotificationCompleteDate = new OptionalDateInputViewModel(completeDate);
            model.NotificationAcknowledgedDate = new OptionalDateInputViewModel(acknowledgedDate);

            return model;
        }
    }
}
