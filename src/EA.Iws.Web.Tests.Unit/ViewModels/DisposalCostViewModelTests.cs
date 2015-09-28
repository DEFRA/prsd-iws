namespace EA.Iws.Web.Tests.Unit.ViewModels
{
    using Areas.NotificationApplication.ViewModels.WasteRecovery;
    using Core.Shared;
    using TestHelpers;
    using Xunit;

    public class DisposalCostViewModelTests
    {
        [Fact]
        public void NoCostProvided_ValidationError()
        {
            var viewModel = new DisposalCostViewModel();
            viewModel.Units = ValuePerWeightUnits.Kilogram;
            
            Assert.True(ViewModelValidator.ValidateViewModel(viewModel).Count > 0);
        }

        [Fact]
        public void NoUnitsProvided_ValidationError()
        {
            var viewModel = new DisposalCostViewModel();
            viewModel.Amount = "1";

            Assert.True(ViewModelValidator.ValidateViewModel(viewModel).Count > 0);
        }

        [Theory]
        [InlineData("-78")]
        [InlineData("not a number")]
        [InlineData("1.4.5")]
        [InlineData("1,345,,234.56")]
        [InlineData("1,2.34")]
        public void CostInvalidFormat_ValidationError(string amount)
        {
            var viewModel = new DisposalCostViewModel();
            viewModel.Units = ValuePerWeightUnits.Kilogram;
            viewModel.Amount = amount;

            Assert.True(ViewModelValidator.ValidateViewModel(viewModel).Count > 0);
        }
    }
}
