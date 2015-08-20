namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.NotificationAssessment.Controllers;
    using Areas.NotificationAssessment.ViewModels;
    using Core.NotificationAssessment;
    using FakeItEasy;
    using Requests.Admin.NotificationAssessment;
    using Web.ViewModels.Shared;
    using Xunit;

    public class KeyDatesControllerTests
    {
        private readonly IIwsClient client;
        private Guid notificationId = new Guid("65214A82-EE7B-42FF-A4A1-220B2A7E74BB");
        private DateTime notificationReceivedDate = new DateTime(2015, 8, 1);
        private KeyDatesController controller;
        private DateTime paymentReceivedDate = new DateTime(2015, 8, 2);

        public KeyDatesControllerTests()
        {
            client = A.Fake<IIwsClient>();

            A.CallTo(
                () => client.SendAsync(A<string>._, A<GetDates>.That.Matches(p => p.NotificationId == notificationId)))
                .Returns(new NotificationDatesData()
                {
                    NotificationId = notificationId,
                    NotificationReceivedDate = notificationReceivedDate
                });

            controller = new KeyDatesController(() => client);
        }

        [Fact]
        public async Task Index_ValidInput_NoValidationError()
        {
            var result = await SetDecisionRequiredDate(22, 7, 2015) as ViewResult;

            Assert.True(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task Index_ValidInput_DecisionDateSetCorrectly()
        {
            SetDates expectedDates = new SetDates();
            expectedDates.DecisionDate = new DateTime(2015, 7, 22);
            var result = await SetDecisionRequiredDate(22, 07, 2015) as ViewResult;

            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<SetDates>.That.Matches(dates => dates.DecisionDate == expectedDates.DecisionDate))).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Index_FetchesNotificationDates()
        {
            await controller.Index(notificationId);

            A.CallTo(() => client.SendAsync(A<string>._, A<GetDates>.That.Matches(dates => dates.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Index_SetsNotificationReceivedDateIfPopulated()
        {
            var controller = new KeyDatesController(() => client);

            var result = await controller.Index(notificationId) as ViewResult;

            Assert.Equal(notificationReceivedDate, ((DateInputViewModel)result.Model).NotificationReceivedDate.AsDateTime());
        }

        [Fact]
        public async Task NotificationReceived_ValidInput_NoValidationError()
        {
            var model = new DateInputViewModel();
            model.NotificationReceivedDate = new OptionalDateInputViewModel(notificationReceivedDate);
            model.Command = DateInputViewModel.NotificationReceived;

            var controller = GetMockAssessmentController(model);

            var result = await controller.Index(model) as ViewResult;

            Assert.True(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task NotificationReceived_InvalidInput_ValidationError()
        {
            var model = new DateInputViewModel();
            model.Command = DateInputViewModel.NotificationReceived;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NotificationReceivedDate"));
        }

        [Fact]
        public async Task NotificationReceived_ValidInput_CallsClient()
        {
            var model = new DateInputViewModel();
            model.NotificationReceivedDate = new OptionalDateInputViewModel(notificationReceivedDate);
            model.Command = DateInputViewModel.NotificationReceived;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            A.CallTo(
                () =>
                    client.SendAsync(A<string>._,
                        A<SetNotificationReceivedDate>.That.Matches(
                            p =>
                                p.NotificationId == model.NotificationId &&
                                p.NotificationReceivedDate == model.NotificationReceivedDate.AsDateTime().Value)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task PaymentReceived_ValidInput_NoValidationError()
        {
            var model = new DateInputViewModel();
            model.PaymentReceivedDate = new OptionalDateInputViewModel(paymentReceivedDate);
            model.Command = DateInputViewModel.PaymentReceived;

            var controller = GetMockAssessmentController(model);

            var result = await controller.Index(model) as ViewResult;

            Assert.True(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task PaymentReceived_InvalidInput_ValidationError()
        {
            var model = new DateInputViewModel();
            model.Command = DateInputViewModel.PaymentReceived;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("PaymentReceivedDate"));
        }

        [Fact]
        public async Task PaymentReceived_ValidInput_CallsClient()
        {
            var model = new DateInputViewModel();
            model.PaymentReceivedDate = new OptionalDateInputViewModel(paymentReceivedDate);
            model.Command = DateInputViewModel.PaymentReceived;

            var controller = GetMockAssessmentController(model);
            
            await controller.Index(model);

            A.CallTo(
                () =>
                    client.SendAsync(A<string>._,
                        A<SetPaymentReceivedDate>.That.Matches(
                            p =>
                                p.NotificationId == model.NotificationId &&
                                p.PaymentReceivedDate == model.PaymentReceivedDate.AsDateTime().Value)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        private Task<ActionResult> SetDecisionRequiredDate(int day, int month, int year)
        {
            DateInputViewModel model = new DateInputViewModel();
            model.DecisionDate = new OptionalDateInputViewModel();

            model.DecisionDate.Day = day;
            model.DecisionDate.Month = month;
            model.DecisionDate.Year = year;

            var controller = GetMockAssessmentController(model);

            return controller.Index(model);
        }

        private KeyDatesController GetMockAssessmentController(object viewModel)
        {
            var assessmentController = new KeyDatesController(() => client);
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
