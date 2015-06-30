namespace EA.Iws.Web.Tests.Unit.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.WasteCodes;
    using Core.WasteCodes;
    using Core.WasteType;
    using FakeItEasy;
    using Xunit;

    public class YcodeHcodeUnClassViewModelTests
    {
        private readonly WasteCodesController controller;

        public YcodeHcodeUnClassViewModelTests()
        {
            controller = GetMockWasteCodesController();
        }

        [Fact]
        public async Task Ycode_ListIsNull_ValidationError()
        {
            var viewModel = GetValidYcodeHcodeUnClassViewModel();
            viewModel.SelectedYcode = string.Empty;
            viewModel.SelectedYcodesList = null;

            ValidateViewModel(viewModel);
            var result = await controller.AddYcodeHcodeAndUnClass(viewModel, "continue", null) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task Ycode_ListIsEmpty_ValidationError()
        {
            var viewModel = GetValidYcodeHcodeUnClassViewModel();
            viewModel.SelectedYcode = string.Empty;
            viewModel.SelectedYcodesList = new List<WasteCodeData>();

            ValidateViewModel(viewModel);
            var result = await controller.AddYcodeHcodeAndUnClass(viewModel, "continue", null) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task Hcode_ListIsNull_ValidationError()
        {
            var viewModel = GetValidYcodeHcodeUnClassViewModel();
            viewModel.SelectedHcode = string.Empty;
            viewModel.SelectedHcodesList = null;

            ValidateViewModel(viewModel);
            var result = await controller.AddYcodeHcodeAndUnClass(viewModel, "continue", null) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task Hcode_ListIsEmpty_ValidationError()
        {
            var viewModel = GetValidYcodeHcodeUnClassViewModel();
            viewModel.SelectedHcode = string.Empty;
            viewModel.SelectedHcodesList = new List<WasteCodeData>();

            ValidateViewModel(viewModel);
            var result = await controller.AddYcodeHcodeAndUnClass(viewModel, "continue", null) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task UnClass_ListIsNull_ValidationError()
        {
            var viewModel = GetValidYcodeHcodeUnClassViewModel();
            viewModel.SelectedUnClass = string.Empty;
            viewModel.SelectedUnClassesList = null;

            ValidateViewModel(viewModel);
            var result = await controller.AddYcodeHcodeAndUnClass(viewModel, "continue", null) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task UnClass_ListIsEmpty_ValidationError()
        {
            var viewModel = GetValidYcodeHcodeUnClassViewModel();
            viewModel.SelectedUnClass = string.Empty;
            viewModel.SelectedUnClassesList = new List<WasteCodeData>();

            ValidateViewModel(viewModel);
            var result = await controller.AddYcodeHcodeAndUnClass(viewModel, "continue", null) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        private static WasteCodesController GetMockWasteCodesController()
        {
            var client = A.Fake<IIwsClient>();

            var controller = new WasteCodesController(() => client);

            return controller;
        }

        private void ValidateViewModel(object viewModel)
        {
            // Mimic the behaviour of the model binder which is responsible for Validating the Model
            var validationContext = new ValidationContext(viewModel, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(viewModel, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
        }

        private static YcodeHcodeAndUnClassViewModel GetValidYcodeHcodeUnClassViewModel()
        {
            const string selectedYcode = "Y1";
            const string selectedHcode = "H1";
            const string selectedUnClass = "1";

            var selectedYcodesList = new List<WasteCodeData>();
            var ylistItem = new WasteCodeData { Code = "Y2" };
            selectedYcodesList.Add(ylistItem);

            var selectedHcodesList = new List<WasteCodeData>();
            var hlistItem = new WasteCodeData { Code = "H2" };
            selectedHcodesList.Add(hlistItem);

            var selectedUnClassesList = new List<WasteCodeData>();
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