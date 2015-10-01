namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationAssessment
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Areas.NotificationAssessment.Controllers;
    using FakeItEasy;
    using Mappings.NotificationAssessment;
    using Prsd.Core.Mediator;

    public class DecisionControllerTests
    {
        private readonly IMediator mediator;

        public DecisionControllerTests()
        {
            mediator = A.Fake<IMediator>();
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
