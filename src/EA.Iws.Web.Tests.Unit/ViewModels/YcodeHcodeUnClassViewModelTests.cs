namespace EA.Iws.Web.Tests.Unit.ViewModels
{
    using Api.Client;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.WasteType;
    using FakeItEasy;
    using Requests.WasteType;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xunit;

    public class YcodeHcodeUnClassViewModelTests
    {
        [Fact]
        public async Task Ycode_ListIsNull_ValidationError()
        {
            var viewModel = GetValidYcodeHcodeUnClassViewModel();
            viewModel.SelectedYcode = string.Empty;
            viewModel.SelectedYcodesList = null;

            var controller = GetMockWasteTypeController(viewModel);
            var result = await controller.AddYcodeHcodeAndUnClass(viewModel, "continue") as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task Ycode_ListIsEmpty_ValidationError()
        {
            var viewModel = GetValidYcodeHcodeUnClassViewModel();
            viewModel.SelectedYcode = string.Empty;
            viewModel.SelectedYcodesList = new List<WasteCodeData>();

            var controller = GetMockWasteTypeController(viewModel);
            var result = await controller.AddYcodeHcodeAndUnClass(viewModel, "continue") as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task Hcode_ListIsNull_ValidationError()
        {
            var viewModel = GetValidYcodeHcodeUnClassViewModel();
            viewModel.SelectedHcode = string.Empty;
            viewModel.SelectedHcodesList = null;

            var controller = GetMockWasteTypeController(viewModel);
            var result = await controller.AddYcodeHcodeAndUnClass(viewModel, "continue") as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task Hcode_ListIsEmpty_ValidationError()
        {
            var viewModel = GetValidYcodeHcodeUnClassViewModel();
            viewModel.SelectedHcode = string.Empty;
            viewModel.SelectedHcodesList = new List<WasteCodeData>();

            var controller = GetMockWasteTypeController(viewModel);
            var result = await controller.AddYcodeHcodeAndUnClass(viewModel, "continue") as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task UnClass_ListIsNull_ValidationError()
        {
            var viewModel = GetValidYcodeHcodeUnClassViewModel();
            viewModel.SelectedUnClass = string.Empty;
            viewModel.SelectedUnClassesList = null;

            var controller = GetMockWasteTypeController(viewModel);
            var result = await controller.AddYcodeHcodeAndUnClass(viewModel, "continue") as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task UnClass_ListIsEmpty_ValidationError()
        {
            var viewModel = GetValidYcodeHcodeUnClassViewModel();
            viewModel.SelectedUnClass = string.Empty;
            viewModel.SelectedUnClassesList = new List<WasteCodeData>();

            var controller = GetMockWasteTypeController(viewModel);
            var result = await controller.AddYcodeHcodeAndUnClass(viewModel, "continue") as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }
        private static WasteTypeController GetMockWasteTypeController(object viewModel)
        {
            var client = A.Fake<IIwsClient>();

            var controller = new WasteTypeController(() => client);
            // Mimic the behaviour of the model binder which is responsible for Validating the Model
            var validationContext = new ValidationContext(viewModel, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(viewModel, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            return controller;
        }

        private static YcodeHcodeAndUnClassViewModel GetValidYcodeHcodeUnClassViewModel()
        {
            const string selectedYcode = "Y1";
            const string selectedHcode = "H1";
            const string selectedUnClass = "1";

            List<WasteCodeData> selectedYcodesList = new List<WasteCodeData>();
            var ylistItem = new WasteCodeData{Code = "Y2"};
            selectedYcodesList.Add(ylistItem);

            List<WasteCodeData> selectedHcodesList = new List<WasteCodeData>();
            var hlistItem = new WasteCodeData { Code = "H2" };
            selectedHcodesList.Add(hlistItem);

            List<WasteCodeData> selectedUnClassesList = new List<WasteCodeData>();
            var unlistItem = new WasteCodeData { Code = "2" };
            selectedUnClassesList.Add(unlistItem);

            var validViewModel = new YcodeHcodeAndUnClassViewModel
            {
                SelectedYcode = selectedYcode,
                SelectedHcode = selectedHcode,
                SelectedUnClass = selectedUnClass,
                SelectedYcodesList = selectedYcodesList,
                SelectedHcodesList = selectedHcodesList,
                SelectedUnClassesList = selectedUnClassesList
            };

            return validViewModel;
        }
    }
}
