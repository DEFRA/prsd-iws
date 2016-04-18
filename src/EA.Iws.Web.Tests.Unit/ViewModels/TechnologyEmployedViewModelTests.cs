namespace EA.Iws.Web.Tests.Unit.ViewModels
{
    using Areas.NotificationApplication.ViewModels.WasteOperations;
    using TestHelpers;
    using Xunit;

    public class TechnologyEmployedViewModelTests
    {
        private readonly string invalidTechnologyEmployed = "This Technology Employed Detail is invalid, because its length is greater than 70 characters.";

        [Fact]
        public void ValidTechnologyEmployed_NoValidationError()
        {
            var viewModel = new TechnologyEmployedViewModel { Details = "valid value" };

            Assert.True(ViewModelValidator.ValidateViewModel(viewModel).Count == 0);
        }

        [Fact]
        public void InvalidTechnologyEmployed_ValidationError()
        {
            var viewModel = new TechnologyEmployedViewModel { Details = invalidTechnologyEmployed };

            Assert.True(ViewModelValidator.ValidateViewModel(viewModel).Count > 0);
        }

        [Fact]
        public void CanNotProvideFurtherDetails_WhenAnnexProvided()
        {
            var viewModel = new TechnologyEmployedViewModel
            {
                Details = invalidTechnologyEmployed,
                AnnexProvided = true,
                FurtherDetails = "any value"
            };

            Assert.True(ViewModelValidator.ValidateViewModel(viewModel).Count > 0);
        }
    }
}
