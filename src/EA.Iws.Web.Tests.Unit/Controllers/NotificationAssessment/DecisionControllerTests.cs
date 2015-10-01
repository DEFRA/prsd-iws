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
    using FakeItEasy;
    using Mappings.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;
    using Web.ViewModels.Shared;
    using Xunit;

    public class DecisionControllerTests
    {
        private readonly IMediator mediator;

        public DecisionControllerTests()
        {
            mediator = A.Fake<IMediator>();
        }

        [Fact]
        public async Task DecisionDate_ValidInput_RedirectsTo_Home()
        {
            var result = await SetDecisionDate(22, 7, 2015, new DecisionViewModel()) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Home", result.RouteValues["controller"]);
            Assert.Equal("NotificationAssessment", result.RouteValues["area"]);
        }

        [Fact]
        public async Task ConsentFromDate_ValidInput_RedirectsTo_Home()
        {
            var result = await SetConsentedFromDate(22, 7, 2015, new DecisionViewModel()) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Home", result.RouteValues["controller"]);
            Assert.Equal("NotificationAssessment", result.RouteValues["area"]);
        }

        [Fact]
        public async Task ConsentToDate_ValidInput_RedirectsTo_Home()
        {
            var result = await SetConsentedToDate(22, 7, 2015, new DecisionViewModel()) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Home", result.RouteValues["controller"]);
            Assert.Equal("NotificationAssessment", result.RouteValues["area"]);
        }

        [Fact]
        public async Task ConsentFromAfterConsentToDate_InvalidInput_ValidationError()
        {
            var decisionViewModel = new DecisionViewModel();
            await SetConsentedToDate(22, 7, 2015, decisionViewModel);
            var result = await SetConsentedFromDate(22, 7, 2015, decisionViewModel);

            Assert.False(((ViewResult)result).ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task DecisionDate_ValidInput_DecisionDateSetCorrectly()
        {
            SetDecision expectedDates = new SetDecision();
            expectedDates.DecisionMade = new DateTime(2015, 7, 22);
            var result = await SetDecisionDate(22, 07, 2015, new DecisionViewModel());

            A.CallTo(() => mediator.SendAsync(A<SetDecision>.That.Matches(dates => dates.DecisionMade == expectedDates.DecisionMade))).MustHaveHappened(Repeated.Exactly.Once);
        }

        private Task<ActionResult> SetDecisionDate(int day, int month, int year, DecisionViewModel model)
        {
            model.DecisionMadeDate = new OptionalDateInputViewModel(new DateTime(year, month, day));

            var controller = GetMockAssessmentController(model);

            return controller.Index(model);
        }

        private Task<ActionResult> SetConsentedFromDate(int day, int month, int year, DecisionViewModel model)
        {
            model.ConsentValidFromDate = new OptionalDateInputViewModel(new DateTime(year, month, day));

            var controller = GetMockAssessmentController(model);

            return controller.Index(model);
        }

        private Task<ActionResult> SetConsentedToDate(int day, int month, int year, DecisionViewModel model)
        {
            model.ConsentValidToDate = new OptionalDateInputViewModel(new DateTime(year, month, day));

            var controller = GetMockAssessmentController(model);

            return controller.Index(model);
        }

        private DecisionController GetMockAssessmentController(object viewModel)
        {
            var decisionController = new DecisionController(mediator, new NotificationAssessmentDecisionDataMap());
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
