namespace EA.Iws.Web.Tests.Unit.ViewModels.Movement
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Areas.Movement.ViewModels.Acceptance;
    using Core.MovementReceipt;
    using Xunit;

    public class ShipmentAcceptanceViewModelTests
    {
        [Fact]
        public void ValidViewModel_Yes_Validates()
        {
            var viewModel = new AcceptanceViewModel();
            viewModel.Decision = Decision.Accepted;

            var result = ValidateViewModel(viewModel);

            Assert.Empty(result);
        }

        [Fact]
        public void ValidViewModel_No_Validates()
        {
            var viewModel = new AcceptanceViewModel();
            viewModel.Decision = Decision.Rejected;
            viewModel.RejectReason = "Rejected because";

            var result = ValidateViewModel(viewModel);

            Assert.Empty(result);
        }

        [Fact]
        public void NoAndNoReason_Invalid()
        {
            var viewModel = new AcceptanceViewModel();
            viewModel.Decision = Decision.Rejected;

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.Equals("Please enter the reason the shipment was rejected"));
        }

        [Fact]
        public void NoAnswer_Invalid()
        {
            var viewModel = new AcceptanceViewModel();

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.Equals("Please answer this question"));
        }

        [Fact]
        public void ReasonTooLong_Invalid()
        {
            var viewModel = new AcceptanceViewModel();
            viewModel.Decision = Decision.Rejected;
            viewModel.RejectReason = GetLongString();

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.Equals("Reason for rejection cannot be longer than 200 characters"));
        }

        private static IEnumerable<ValidationResult> ValidateViewModel(object viewModel)
        {
            var validationContext = new ValidationContext(viewModel, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(viewModel, validationContext, validationResults, false);

            return validationResults;
        }

        public string GetLongString()
        {
            return
                "This is a string that is longer than 200 characters so that the view model " +
                "will be invalid when it is entered in the reason for rejection field.  " +
                "That was one hundred and forty four so I am adding more text to increase the total count.";
        }
    }
}
