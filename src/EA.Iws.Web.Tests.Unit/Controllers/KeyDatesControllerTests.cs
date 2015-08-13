namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.Admin.Controllers;
    using Areas.Admin.ViewModels;
    using Areas.NotificationAssessment.Controllers;
    using Areas.NotificationAssessment.ViewModels;
    using FakeItEasy;
    using Requests.Admin;
    using Web.ViewModels.Shared;
    using Xunit;

    public class KeyDatesControllerTests
    {
        private readonly IIwsClient client;

        public KeyDatesControllerTests()
        {
            client = A.Fake<IIwsClient>();
        }

        [Fact]
        public async Task DecisionDate_ValidInput_NoValidationError()
        {
            var result = await SetDecisionRequiredDate(22, 7, 2015) as ViewResult;

            Assert.True(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task DecisionDate_ValidInput_DecisionDateSetCorrectly()
        {
            SetDates expectedDates = new SetDates();
            expectedDates.DecisionDate = new DateTime(2015, 7, 22);
            var result = await SetDecisionRequiredDate(22, 07, 2015) as ViewResult;

            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<SetDates>.That.Matches(dates => dates.DecisionDate == expectedDates.DecisionDate))).MustHaveHappened(Repeated.Exactly.Once);
        }

        private Task<ViewResult> SetDecisionRequiredDate(int day, int month, int year)
        {
            DateInputViewModel model = new DateInputViewModel();
            model.DecisionDate = new OptionalDateInputViewModel();

            model.DecisionDate.Day = day;
            model.DecisionDate.Month = month;
            model.DecisionDate.Year = year;

            var controller = GetMockAssessmentController(model);

            return controller.DateInput(model);
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
