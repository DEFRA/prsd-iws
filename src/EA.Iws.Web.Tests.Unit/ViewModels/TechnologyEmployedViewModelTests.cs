namespace EA.Iws.Web.Tests.Unit.ViewModels
{
    using System.Threading.Tasks;
    using Areas.NotificationApplication.ViewModels.WasteOperations;
    using TestHelpers;
    using Xunit;

    public class TechnologyEmployedViewModelTests
    {
        private readonly string invalidTechnologyEmployed = "This Technology Employed Detail is invalid, because its length is greater than 70 characters.";

        [Fact]
        public async Task ValidTechnologyEmployed_NoValidationError()
        {
            var viewModel = new TechnologyEmployedViewModel();
            viewModel.Details = "valid value";

            Assert.True(ViewModelValidator.ValidateViewModel(viewModel).Count == 0);
        }

        [Fact]
        public async Task InvalidTechnologyEmployed_ValidationError()
        {
            var viewModel = new TechnologyEmployedViewModel();
            viewModel.Details = invalidTechnologyEmployed;

            Assert.True(ViewModelValidator.ValidateViewModel(viewModel).Count > 0);
        }

        [Fact]
        public async Task CanNotProvideFurtherDetails_WhenAnnexProvided()
        {
            var viewModel = new TechnologyEmployedViewModel();
            viewModel.Details = invalidTechnologyEmployed;
            viewModel.AnnexProvided = true;
            viewModel.FurtherDetails = "any value";

            Assert.True(ViewModelValidator.ValidateViewModel(viewModel).Count > 0);
        }
    }
}
