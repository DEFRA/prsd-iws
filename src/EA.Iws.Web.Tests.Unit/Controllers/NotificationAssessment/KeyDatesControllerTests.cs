namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.NotificationAssessment.Controllers;
    using Areas.NotificationAssessment.ViewModels;
    using Core.NotificationAssessment;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;
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
                () => mediator.SendAsync(A<GetDates>.That.Matches(p => p.NotificationId == notificationId)))
                .Returns(new NotificationDatesData()
                {
                    NotificationId = notificationId,
                    NotificationReceivedDate = notificationReceivedDate,
                    PaymentReceivedDate = paymentReceivedDate,
                    AcknowledgedDate = acknowledgedDate,
                    DecisionRequiredDate = decisionRequiredDate
                });

            controller = new KeyDatesController(mediator);
        }

        [Fact]
        public async Task Index_ValidInput_NoValidationError()
        {
            var model = GetDecisionRequiredDateModel(22, 7, 2015);
            model.Command = KeyDatesStatusEnum.AssessmentCommenced;

            await controller.Index(model);

            Assert.True(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task Index_FetchesNotificationDates()
        {
            await controller.Index(notificationId, 0);

            A.CallTo(() => mediator.SendAsync(A<GetDates>.That.Matches(dates => dates.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Index_SetsNotificationReceivedDateIfPopulated()
        {
            var controller = new KeyDatesController(mediator);

            var result = await controller.Index(notificationId, 0) as ViewResult;

            Assert.Equal(notificationReceivedDate, ((DateInputViewModel)result.Model).NotificationReceivedDate.AsDateTime());
        }

        [Fact]
        public async Task Index_SetsPaymentReceivedDateIfPopulated()
        {
            var controller = new KeyDatesController(mediator);

            var result = await controller.Index(notificationId, 0) as ViewResult;

            Assert.Equal(paymentReceivedDate, ((DateInputViewModel)result.Model).PaymentReceivedDate.AsDateTime());
        }

        [Fact]
        public async Task Index_SetsAcknowledgedDateIfPopulated()
        {
            var controller = new KeyDatesController(mediator);

            var result = await controller.Index(notificationId, 0) as ViewResult;

            Assert.Equal(acknowledgedDate, ((DateInputViewModel)result.Model).NotificationAcknowledgedDate.AsDateTime());
        }

        [Fact]
        public async Task Index_SetsDecisionRequiredByDateIfPopulated()
        {
            var controller = new KeyDatesController(mediator);

            var result = await controller.Index(notificationId, 0) as ViewResult;

            Assert.Equal(decisionRequiredDate, ((DateInputViewModel)result.Model).DecisionDate.AsDateTime());
        }

        [Fact]
        public async Task NotificationReceived_ValidInput_NoValidationError()
        {
            var model = new DateInputViewModel();
            model.NewDate = new OptionalDateInputViewModel(notificationReceivedDate);
            model.Command = KeyDatesStatusEnum.NotificationReceived;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task NotificationReceived_InvalidInput_ValidationError()
        {
            var model = new DateInputViewModel();
            model.Command = KeyDatesStatusEnum.NotificationReceived;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationReceived_ValidInput_CallsClient()
        {
            var model = new DateInputViewModel();
            model.NewDate = new OptionalDateInputViewModel(notificationReceivedDate);
            model.Command = KeyDatesStatusEnum.NotificationReceived;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            A.CallTo(() => mediator.SendAsync(A<SetNotificationReceivedDate>
                    .That.Matches(p => p.NotificationId == model.NotificationId &&
                                p.NotificationReceivedDate == model.NewDate.AsDateTime().Value)))
                    .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task PaymentReceived_ValidInput_NoValidationError()
        {
            var model = new DateInputViewModel();
            model.NewDate = new OptionalDateInputViewModel(paymentReceivedDate);
            model.Command = KeyDatesStatusEnum.PaymentReceived;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task PaymentReceived_InvalidInput_ValidationError()
        {
            var model = new DateInputViewModel();
            model.Command = KeyDatesStatusEnum.PaymentReceived;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task PaymentReceived_ValidInput_CallsClient()
        {
            var model = new DateInputViewModel();
            model.NewDate = new OptionalDateInputViewModel(paymentReceivedDate);
            model.Command = KeyDatesStatusEnum.PaymentReceived;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            A.CallTo(() => mediator.SendAsync(A<SetPaymentReceivedDate>
                .That.Matches(p => p.NotificationId == model.NotificationId &&
                                p.PaymentReceivedDate == model.NewDate.AsDateTime().Value)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task AssessmentCommenced_ValidInput_NoValidationError()
        {
            var model = new DateInputViewModel();
            model.NewDate = new OptionalDateInputViewModel(commencementDate);
            model.NameOfOfficer = "Officer";
            model.Command = KeyDatesStatusEnum.AssessmentCommenced;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task AssessmentCommenced_InvalidInput_ValidationError()
        {
            var model = new DateInputViewModel();
            model.Command = KeyDatesStatusEnum.AssessmentCommenced;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate") && controller.ModelState.ContainsKey("NameOfOfficer"));
        }

        [Fact]
        public async Task AssessmentCommenced_OfficerNameToLong_ValidationError()
        {
            var model = new DateInputViewModel();
            model.NewDate = new OptionalDateInputViewModel(commencementDate);
            model.NameOfOfficer = GetLongString();
            model.Command = KeyDatesStatusEnum.AssessmentCommenced;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NameOfOfficer"));
        }

        [Fact]
        public async Task AssessmentCommenced_ValidInput_CallsClient()
        {
            var model = new DateInputViewModel();
            model.NewDate = new OptionalDateInputViewModel(commencementDate);
            model.NameOfOfficer = "Officer";
            model.Command = KeyDatesStatusEnum.AssessmentCommenced;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            A.CallTo(() => mediator.SendAsync(A<SetCommencedDate>
                .That.Matches(p => p.NotificationId == model.NotificationId &&
                                p.CommencementDate == model.NewDate.AsDateTime().Value &&
                                p.NameOfOfficer == model.NameOfOfficer)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task NotificationComplete_ValidInput_NoValidationError()
        {
            var model = new DateInputViewModel();
            model.NewDate = new OptionalDateInputViewModel(completeDate);
            model.Command = KeyDatesStatusEnum.NotificationComplete;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task NotificationComplete_InvalidInput_ValidationError()
        {
            var model = new DateInputViewModel();
            model.Command = KeyDatesStatusEnum.NotificationComplete;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationComplete_ValidInput_CallsClient()
        {
            var model = new DateInputViewModel();
            model.NewDate = new OptionalDateInputViewModel(completeDate);
            model.Command = KeyDatesStatusEnum.NotificationComplete;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            A.CallTo(() => mediator.SendAsync(A<SetNotificationCompleteDate>
                .That.Matches(p => p.NotificationId == model.NotificationId &&
                                p.NotificationCompleteDate == model.NewDate.AsDateTime().Value)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task NotificationTransmitted_ValidInput_NoValidationError()
        {
            var model = new DateInputViewModel();
            model.NewDate = new OptionalDateInputViewModel(transmittedDate);
            model.Command = KeyDatesStatusEnum.NotificationTransmitted;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task NotificationTransmitted_InvalidInput_ValidationError()
        {
            var model = new DateInputViewModel();
            model.Command = KeyDatesStatusEnum.NotificationTransmitted;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationTransmitted_ValidInput_CallsClient()
        {
            var model = new DateInputViewModel();
            model.NewDate = new OptionalDateInputViewModel(transmittedDate);
            model.Command = KeyDatesStatusEnum.NotificationTransmitted;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            A.CallTo(() => mediator.SendAsync(A<SetNotificationTransmittedDate>
                .That.Matches(p => p.NotificationId == model.NotificationId &&
                                p.NotificationTransmittedDate == model.NewDate.AsDateTime().Value)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task NotificationAcknowledged_ValidInput_NoValidationError()
        {
            var model = new DateInputViewModel();
            model.NewDate = new OptionalDateInputViewModel(acknowledgedDate);
            model.Command = KeyDatesStatusEnum.NotificationAcknowledged;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task NotificationAcknowledged_InvalidInput_ValidationError()
        {
            var model = new DateInputViewModel();
            model.Command = KeyDatesStatusEnum.NotificationAcknowledged;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationAcknowledged_ValidInput_CallsClient()
        {
            var model = new DateInputViewModel();
            model.NewDate = new OptionalDateInputViewModel(acknowledgedDate);
            model.Command = KeyDatesStatusEnum.NotificationAcknowledged;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            A.CallTo(() => mediator.SendAsync(A<SetNotificationAcknowledgedDate>
                .That.Matches(p => p.NotificationId == model.NotificationId &&
                                p.AcknowledgedDate == model.NewDate.AsDateTime().Value)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        private DateInputViewModel GetDecisionRequiredDateModel(int day, int month, int year)
        {
            DateInputViewModel model = new DateInputViewModel();
            model.DecisionDate = new OptionalDateInputViewModel();

            model.DecisionDate.Day = day;
            model.DecisionDate.Month = month;
            model.DecisionDate.Year = year;

            return model;
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

        private string GetLongString()
        {
            return "A string longer than 256 characters asdfasdf asdfasdf asfasdf asdfasdf asdfasdfs sdfas alksdfjh asdkfhj " +
                   "asdklfhj  alksdhf alskdf asdf asd fa sdf a sdf as df asdfa as df asdf a sdf asdf  asdf a sdf asdf asdf " +
                   "asdf asdf asd fa sdf as df asdf a sdf asdfasdfasd7";
        }
    }
}
