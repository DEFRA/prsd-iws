namespace EA.Iws.Web.Tests.Unit.ViewModels
{
    using System.Threading.Tasks;
    using Areas.NotificationApplication.ViewModels.NotificationApplication;
    using TestHelpers;
    using Xunit;

    public class ReasonForExportViewModelTests
    {
        private readonly string invalidReasonOfExport = "This reason of export is invalid, because its length is greater than 70 characters.";

        [Fact]
        public async Task ValidReasonOfExport_NoValidationError()
        {
            var viewModel = new ReasonForExportViewModel();
            viewModel.ReasonForExport = "valid value";

            Assert.True(ViewModelValidator.ValidateViewModel(viewModel).Count == 0);
        }

        [Fact]
        public async Task InvalidReasonOfExport_ValidationError()
        {
            var viewModel = new ReasonForExportViewModel();
            viewModel.ReasonForExport = invalidReasonOfExport;

            Assert.True(ViewModelValidator.ValidateViewModel(viewModel).Count > 0);
        }
    }
}
