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
    using FakeItEasy;
    using Requests.Admin;
    using Xunit;

    public class DecisionControllerTests
    {
        private readonly IIwsClient client;

        public DecisionControllerTests()
        {
            client = A.Fake<IIwsClient>();
        }

        [Fact]
        public async Task DecisionDate_ValidInput_NoValidationError()
        {
            var result = await SetDecisionDate(22, 7, 2015, new DecisionViewModel());

            Assert.True(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task ConsentFromDate_ValidInput_NoValidationError()
        {
            var result = await SetConsentedFromDate(22, 7, 2015, new DecisionViewModel());

            Assert.True(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task ConsentToDate_ValidInput_NoValidationError()
        {
            var result = await SetConsentedToDate(22, 7, 2015, new DecisionViewModel());

            Assert.True(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task ConsentFromAfterConsentToDate_InvalidInput_ValidationError()
        {
            var decisionViewModel = new DecisionViewModel();
            await SetConsentedToDate(22, 7, 2015, decisionViewModel);
            var result = await SetConsentedFromDate(22, 7, 2015, decisionViewModel);

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Theory]
        [InlineData(40, 13, 2015)]
        [InlineData(-25, -2, -2015)]
        public async Task DecisionDate_InvalidInput_ValidationError(int day, int month, int year)
        {
            var result = await SetDecisionDate(day, month, year, new DecisionViewModel());

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task DecisionDate_ValidInput_DecisionDateSetCorrectly()
        {
            SetDecision expectedDates = new SetDecision();
            expectedDates.DecisionMade = new DateTime(2015, 7, 22);
            var result = await SetDecisionDate(22, 07, 2015, new DecisionViewModel());

            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<SetDecision>.That.Matches(dates => dates.DecisionMade == expectedDates.DecisionMade))).MustHaveHappened(Repeated.Exactly.Once);
        }

        private Task<ViewResult> SetDecisionDate(int day, int month, int year, DecisionViewModel model)
        {
            model.DecisionMadeDay = day;
            model.DecisionMadeMonth = month;
            model.DecisionMadeYear = year;

            var controller = GetMockAssessmentController(model);

            return controller.Decision(model);
        }

        private Task<ViewResult> SetConsentedFromDate(int day, int month, int year, DecisionViewModel model)
        {
            model.ConsentValidFromDay = day;
            model.ConsentValidFromMonth = month;
            model.ConsentValidFromYear = year;

            var controller = GetMockAssessmentController(model);

            return controller.Decision(model);
        }

        private Task<ViewResult> SetConsentedToDate(int day, int month, int year, DecisionViewModel model)
        {
            model.ConsentValidToDay = day;
            model.ConsentValidToMonth = month;
            model.ConsentValidToYear = year;

            var controller = GetMockAssessmentController(model);

            return controller.Decision(model);
        }

        private DecisionController GetMockAssessmentController(object viewModel)
        {
            var decisionController = new DecisionController(() => client);
            // Mimic the behaviour of the model binder which is responsible for Validating the Model
            var validationContext = new ValidationContext(viewModel, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(viewModel, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                decisionController.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            return decisionController;
        }
    }
}
