﻿namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.AdminExportAssessment.Controllers;
    using Areas.AdminExportAssessment.ViewModels;
    using Core.NotificationAssessment;
    using EA.Iws.Web.Infrastructure.AdditionalCharge;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;
    using Requests.NotificationAssessment;
    using Web.Infrastructure.Authorization;
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
        private readonly AuthorizationService authorizationService;
        private readonly IAdditionalChargeService additionalChargeService;

        public KeyDatesControllerTests()
        {
            mediator = A.Fake<IMediator>();
            authorizationService = A.Fake<AuthorizationService>();
            additionalChargeService = A.Fake<IAdditionalChargeService>();

            A.CallTo(
                () => mediator.SendAsync(A<GetKeyDatesSummaryInformation>.That.Matches(p => p.NotificationId == notificationId)))
                .Returns(new KeyDatesSummaryData
                {
                    Dates = new NotificationDatesData
                    {
                        NotificationId = notificationId,
                        NotificationReceivedDate = notificationReceivedDate,
                        PaymentReceivedDate = paymentReceivedDate,
                        AcknowledgedDate = acknowledgedDate,
                        DecisionRequiredDate = decisionRequiredDate
                    }
                });

            controller = new KeyDatesController(mediator, authorizationService, additionalChargeService);
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

            A.CallTo(() => mediator.SendAsync(A<GetKeyDatesSummaryInformation>.That.Matches(dates => dates.NotificationId == notificationId)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Index_SetsNotificationReceivedDateIfPopulated()
        {
            var result = await controller.Index(notificationId, 0) as ViewResult;

            Assert.Equal(notificationReceivedDate, ((DateInputViewModel)result.Model).NotificationReceivedDate.AsDateTime());
        }

        [Fact]
        public async Task Index_SetsPaymentReceivedDateIfPopulated()
        {
            var result = await controller.Index(notificationId, 0) as ViewResult;

            Assert.Equal(paymentReceivedDate, ((DateInputViewModel)result.Model).PaymentReceivedDate);
        }

        [Fact]
        public async Task Index_SetsAcknowledgedDateIfPopulated()
        {
            var result = await controller.Index(notificationId, 0) as ViewResult;

            Assert.Equal(acknowledgedDate, ((DateInputViewModel)result.Model).NotificationAcknowledgedDate.AsDateTime());
        }

        [Fact]
        public async Task Index_SetsDecisionRequiredByDateIfPopulated()
        {
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
            await Date_InvalidInput_ValidationError(KeyDatesStatusEnum.NotificationReceived);
        }

        [Fact]
        public async Task NotificationReceived_InFuture_Invalid()
        {
            var model = GetValidViewModel();
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow.AddDays(1));
            model.Command = KeyDatesStatusEnum.NotificationReceived;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task PaymentReceived_InvalidInput_ValidationError()
        {
            await Date_InvalidInput_ValidationError(KeyDatesStatusEnum.PaymentReceived);
        }

        [Fact]
        public async Task AssessmentCommenced_ValidInput_NoValidationError()
        {
            var model = GetValidViewModel();
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
        public async Task AssessmentCommenced_InFuture_Invalid()
        {
            var model = GetValidViewModel();
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow.AddDays(1));
            model.Command = KeyDatesStatusEnum.AssessmentCommenced;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task AssessmentCommenced_NotificationReceivedDate_MustBePresent()
        {
            var model = GetValidViewModel();
            model.NotificationCompleteDate = null;
            model.NotificationReceivedDate = null;
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = KeyDatesStatusEnum.AssessmentCommenced;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task AssessmentCommenced_PaymentReceivedDate_MustBePresent()
        {
            var model = GetValidViewModel();
            model.NotificationCompleteDate = null;
            model.PaymentReceivedDate = null;
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = KeyDatesStatusEnum.AssessmentCommenced;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task AssessmentCommenced_NotBefore_NotificationReceivedDate()
        {
            var model = GetValidViewModel();
            model.NotificationCompleteDate = null;
            model.NewDate = new OptionalDateInputViewModel(new DateTime(2015, 7, 2));
            model.Command = KeyDatesStatusEnum.AssessmentCommenced;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task AssessmentCommenced_NotBefore_PaymentReceivedDate()
        {
            var model = GetValidViewModel();
            model.NotificationCompleteDate = null;
            model.NewDate = new OptionalDateInputViewModel(new DateTime(2015, 8, 1));
            model.Command = KeyDatesStatusEnum.AssessmentCommenced;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationComplete_ValidInput_NoValidationError()
        {
            var model = GetValidViewModel();
            model.NewDate = new OptionalDateInputViewModel(completeDate);
            model.Command = KeyDatesStatusEnum.NotificationComplete;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task NotificationComplete_InvalidInput_ValidationError()
        {
            await Date_InvalidInput_ValidationError(KeyDatesStatusEnum.NotificationComplete);
        }

        [Fact]
        public async Task NotificationComplete_InFuture_Invalid()
        {
            var model = GetValidViewModel();
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow.AddDays(1));
            model.Command = KeyDatesStatusEnum.NotificationComplete;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationComplete_NotificationReceivedDate_MustBePresent()
        {
            var model = GetValidViewModel();
            model.NotificationCompleteDate = null;
            model.NotificationReceivedDate = null;
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = KeyDatesStatusEnum.NotificationComplete;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationComplete_PaymentReceivedDate_MustBePresent()
        {
            var model = GetValidViewModel();
            model.NotificationCompleteDate = null;
            model.PaymentReceivedDate = null;
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = KeyDatesStatusEnum.NotificationComplete;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationComplete_AssessmentStartedDate_MustBePresent()
        {
            var model = GetValidViewModel();
            model.NotificationCompleteDate = null;
            model.CommencementDate = null;
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = KeyDatesStatusEnum.NotificationComplete;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationComplete_NotBefore_NotificationReceivedDate()
        {
            var model = GetValidViewModel();
            model.NotificationCompleteDate = null;
            model.NewDate = new OptionalDateInputViewModel(new DateTime(2015, 7, 2));
            model.Command = KeyDatesStatusEnum.NotificationComplete;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationComplete_NotBefore_PaymentReceivedDate()
        {
            var model = GetValidViewModel();
            model.NotificationCompleteDate = null;
            model.NewDate = new OptionalDateInputViewModel(new DateTime(2015, 8, 1));
            model.Command = KeyDatesStatusEnum.NotificationComplete;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationComplete_CanBeBefore_AssessmentStartedDate()
        {
            var model = GetValidViewModel();
            model.NotificationCompleteDate = null;
            model.NewDate = new OptionalDateInputViewModel(new DateTime(2015, 8, 2));
            model.Command = KeyDatesStatusEnum.NotificationComplete;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.False(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationTransmitted_ValidInput_NoValidationError()
        {
            var model = GetValidViewModel();
            model.NewDate = new OptionalDateInputViewModel(transmittedDate);
            model.Command = KeyDatesStatusEnum.NotificationTransmitted;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task NotificationTransmitted_InvalidInput_ValidationError()
        {
            await Date_InvalidInput_ValidationError(KeyDatesStatusEnum.NotificationTransmitted);
        }

        [Fact]
        public async Task NotificationTransmitted_ValidInput_CallsClient()
        {
            var model = GetValidViewModel();
            model.NewDate = new OptionalDateInputViewModel(transmittedDate);
            model.Command = KeyDatesStatusEnum.NotificationTransmitted;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            A.CallTo(() => mediator.SendAsync(A<SetNotificationTransmittedDate>
                .That.Matches(p => p.NotificationId == model.NotificationId &&
                                p.NotificationTransmittedDate == model.NewDate.AsDateTime().Value)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task NotificationTransmitted_InFuture_Invalid()
        {
            var model = GetValidViewModel();
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow.AddDays(1));
            model.Command = KeyDatesStatusEnum.NotificationTransmitted;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationTransmitted_NotificationReceivedDate_MustBePresent()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.NotificationReceivedDate = null;
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = KeyDatesStatusEnum.NotificationTransmitted;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationTransmitted_PaymentReceivedDate_MustBePresent()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.PaymentReceivedDate = null;
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = KeyDatesStatusEnum.NotificationTransmitted;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationTransmitted_AssessmentStartedDate_MustBePresent()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.CommencementDate = null;
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = KeyDatesStatusEnum.NotificationTransmitted;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationTransmitted_NotificationCompleteDate_MustBePresent()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.NotificationCompleteDate = null;
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = KeyDatesStatusEnum.NotificationTransmitted;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationTransmitted_NotBefore_NotificationReceivedDate()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.NewDate = new OptionalDateInputViewModel(new DateTime(2015, 7, 2));
            model.Command = KeyDatesStatusEnum.NotificationTransmitted;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationTransmitted_NotBefore_PaymentReceivedDate()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.NewDate = new OptionalDateInputViewModel(new DateTime(2015, 8, 1));
            model.Command = KeyDatesStatusEnum.NotificationTransmitted;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationTransmitted_NotBefore_AssessmentStartedDate()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.NewDate = new OptionalDateInputViewModel(new DateTime(2015, 8, 2));
            model.Command = KeyDatesStatusEnum.NotificationTransmitted;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationTransmitted_NotBefore_NotificationCompleteDate()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.NewDate = new OptionalDateInputViewModel(new DateTime(2015, 8, 19));
            model.Command = KeyDatesStatusEnum.NotificationTransmitted;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationAcknowledged_ValidInput_NoValidationError()
        {
            var model = GetValidViewModel();
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = KeyDatesStatusEnum.NotificationAcknowledged;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task NotificationAcknowledged_InvalidInput_ValidationError()
        {
            await Date_InvalidInput_ValidationError(KeyDatesStatusEnum.NotificationAcknowledged);
        }

        [Fact]
        public async Task NotificationAcknowledged_ValidInput_CallsClient()
        {
            var model = GetValidViewModel();
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = KeyDatesStatusEnum.NotificationAcknowledged;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            A.CallTo(() => mediator.SendAsync(A<SetNotificationAcknowledgedDate>
                .That.Matches(p => p.NotificationId == model.NotificationId &&
                                p.AcknowledgedDate == model.NewDate.AsDateTime().Value)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task NotificationAcknowledged_InFuture_Invalid()
        {
            var model = GetValidViewModel();
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow.AddDays(1));
            model.Command = KeyDatesStatusEnum.NotificationAcknowledged;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationAcknowledged_NotificationReceivedDate_MustBePresent()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.NotificationReceivedDate = null;
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = KeyDatesStatusEnum.NotificationAcknowledged;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationAcknowledged_PaymentReceivedDate_MustBePresent()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.PaymentReceivedDate = null;
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = KeyDatesStatusEnum.NotificationAcknowledged;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationAcknowledged_AssessmentStartedDate_MustBePresent()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.CommencementDate = null;
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = KeyDatesStatusEnum.NotificationAcknowledged;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationAcknowledged_NotificationCompleteDate_MustBePresent()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.NotificationCompleteDate = null;
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = KeyDatesStatusEnum.NotificationAcknowledged;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationAcknowledged_TransmittedOnDate_MustBePresent()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.NotificationTransmittedDate = null;
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow);
            model.Command = KeyDatesStatusEnum.NotificationAcknowledged;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationAcknowledged_NotBefore_NotificationReceivedDate()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.NewDate = new OptionalDateInputViewModel(new DateTime(2015, 7, 2));
            model.Command = KeyDatesStatusEnum.NotificationAcknowledged;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationAcknowledged_NotBefore_PaymentReceivedDate()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.NewDate = new OptionalDateInputViewModel(new DateTime(2015, 8, 1));
            model.Command = KeyDatesStatusEnum.NotificationAcknowledged;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationAcknowledged_NotBefore_AssessmentStartedDate()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.NewDate = new OptionalDateInputViewModel(new DateTime(2015, 8, 2));
            model.Command = KeyDatesStatusEnum.NotificationAcknowledged;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationAcknowledged_NotBefore_NotificationCompleteDate()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.NewDate = new OptionalDateInputViewModel(new DateTime(2015, 8, 19));
            model.Command = KeyDatesStatusEnum.NotificationAcknowledged;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }

        [Fact]
        public async Task NotificationAcknowledged_NotBefore_TransmittedOnDate()
        {
            var model = GetValidViewModel();
            model.NotificationAcknowledgedDate = null;
            model.NewDate = new OptionalDateInputViewModel(new DateTime(2015, 8, 20));
            model.Command = KeyDatesStatusEnum.NotificationAcknowledged;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
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
            var assessmentController = new KeyDatesController(mediator, authorizationService, additionalChargeService);
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

        private DateInputViewModel GetValidViewModel()
        {
            var model = new DateInputViewModel();

            model.NotificationReceivedDate = new OptionalDateInputViewModel(notificationReceivedDate);
            model.PaymentReceivedDate = paymentReceivedDate;
            model.CommencementDate = new OptionalDateInputViewModel(commencementDate);
            model.NotificationCompleteDate = new OptionalDateInputViewModel(completeDate);
            model.NotificationTransmittedDate = new OptionalDateInputViewModel(transmittedDate);
            model.NewDate = new OptionalDateInputViewModel(SystemTime.UtcNow.AddDays(1));

            return model;
        }

        private async Task Date_InvalidInput_ValidationError(KeyDatesStatusEnum command)
        {
            var model = new DateInputViewModel();
            model.Command = command;

            var controller = GetMockAssessmentController(model);

            await controller.Index(model);

            Assert.True(controller.ModelState.ContainsKey("NewDate"));
        }
    }
}
