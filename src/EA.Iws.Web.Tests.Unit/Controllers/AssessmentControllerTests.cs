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
    using Areas.NotificationApplication.Controllers;
    using Core.CustomsOffice;
    using FakeItEasy;
    using Prsd.Core.Web.OAuth;
    using Requests.Admin;
    using Requests.CustomsOffice;
    using Xunit;

    public class AssessmentControllerTests
    {
        private readonly IIwsClient client;

        public AssessmentControllerTests()
        {
            client = A.Fake<IIwsClient>();
        }

        [Fact]
        public async Task DecisionDate_ValidInput_NoValidationError()
        {
            var result = await SetDecisionRequiredDate(22, 7, 2015) as ViewResult;

            Assert.True(result.ViewData.ModelState.IsValid);
        }

        [Theory]
        [InlineData(40, 13, 2015)]
        [InlineData(-25, -2, -2015)]
        public async Task DecisionDate_InvalidInput_ValidationError(int day, int month, int year)
        {
            var result = await SetDecisionRequiredDate(day, month, year) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
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
            model.DecisionDay = day;
            model.DecisionMonth = month;
            model.DecisionYear = year;

            var controller = GetMockAssessmentController(model);

            return controller.Continue(model);
        }

        private Areas.Admin.Controllers.AssessmentController GetMockAssessmentController(object viewModel)
        {
            var assessmentController = new AssessmentController(() => client);
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
