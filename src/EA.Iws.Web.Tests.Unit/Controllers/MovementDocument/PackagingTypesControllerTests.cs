namespace EA.Iws.Web.Tests.Unit.Controllers.MovementDocument
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.PackagingType;
    using FakeItEasy;
    using Iws.TestHelpers.Helpers;
    using Requests.Movement;
    using Web.Areas.MovementDocument.Controllers;
    using Web.Areas.MovementDocument.ViewModels;
    using Web.ViewModels.Shared;
    using Xunit;

    public class PackagingTypesControllerTests
    {
        private readonly IIwsClient client;
        private readonly Guid movementId = new Guid("C1C159CB-AF37-4465-A66E-953779158B61");
        private readonly PackagingTypesController controller;

        public PackagingTypesControllerTests()
        {
            client = A.Fake<IIwsClient>();
            controller = new PackagingTypesController(() => client);
        }

        [Fact]
        public async Task Index_Get_CreatesValidViewModel()
        {
            SetUpPackagingData(new[]
            {
                PackagingType.PressureReceptacle
            });

            var result = await controller.Index(movementId) as ViewResult;

            Assert.IsAssignableFrom(typeof(PackagingTypesViewModel), result.Model);

            var resultMovementId = ((PackagingTypesViewModel)result.Model).MovementId;
            var resultPackagingTypes = ((PackagingTypesViewModel)result.Model).PackagingTypes.PossibleValues;

            Assert.Equal(movementId, resultMovementId);
            Assert.Equal(1, resultPackagingTypes.Count);
            Assert.True(resultPackagingTypes.Select(li => li.Text).Contains("Pressure receptacle"));
        }

        [Fact]
        public async Task Index_Get_PossibleValues_MatchValuesFromNotification()
        {
            SetUpPackagingData(new[] 
            { 
                PackagingType.Bag, 
                PackagingType.Drum, 
                PackagingType.Jerrican, 
                PackagingType.CompositePackaging 
            });

            var result = await controller.Index(movementId) as ViewResult;
            var possibleValuesFromViewModel = ((PackagingTypesViewModel)result.Model).PackagingTypes.PossibleValues;

            Assert.Equal(4, possibleValuesFromViewModel.Count);
            Assert.True(possibleValuesFromViewModel.Select(li => li.Text).Contains("Bag")
                && possibleValuesFromViewModel.Select(li => li.Text).Contains("Drum")
                && possibleValuesFromViewModel.Select(li => li.Text).Contains("Jerrican")
                && possibleValuesFromViewModel.Select(li => li.Text).Contains("Composite packaging"));
        }

        [Fact]
        public async Task Index_Get_PossibleValues_Other_ContainsDescription()
        {
            SetUpPackagingData(new[] { PackagingType.Other }, otherDescription: "Carrier bag");

            var result = await controller.Index(movementId) as ViewResult;
            var possibleValuesFromViewModel = ((PackagingTypesViewModel)result.Model).PackagingTypes.PossibleValues;

            Assert.Equal(1, possibleValuesFromViewModel.Count);
            Assert.True(possibleValuesFromViewModel.Select(li => li.Text).Contains("Other - Carrier bag"));
        }

        [Fact]
        public async Task Index_Get_PossibleValues_OrderedByEnumValue()
        {
            SetUpPackagingData(new[]
            {
                PackagingType.WoodenBarrel,
                PackagingType.Drum,
                PackagingType.Jerrican
            });

            var result = await controller.Index(movementId) as ViewResult;
            var possibleValuesFromViewModel = ((PackagingTypesViewModel)result.Model).PackagingTypes.PossibleValues;

            Assert.Collection(possibleValuesFromViewModel,
                item => Assert.Equal("Drum", possibleValuesFromViewModel.ElementAt(0).Text),
                item => Assert.Equal("Wooden barrel", possibleValuesFromViewModel.ElementAt(1).Text),
                item => Assert.Equal("Jerrican", possibleValuesFromViewModel.ElementAt(2).Text));
        }

        [Fact]
        public async Task Index_Get_PreviouslySavedValues_ArePreSelected()
        {
            var availablePackagingData = new[]
            {
                PackagingType.Bag,
                PackagingType.Box,
                PackagingType.Jerrican
            };
            var selectedPackagingData = new[]
            {
                PackagingType.Bag
            };

            SetUpPackagingData(availablePackagingData, selectedPackagingData);

            var result = await controller.Index(movementId) as ViewResult;
            var possibleValuesFromViewModel = ((PackagingTypesViewModel)result.Model).PackagingTypes.PossibleValues;

            Assert.Equal(new[] { "Bag" }.ToList(), 
                possibleValuesFromViewModel.Where(p => p.Selected).Select(p => p.Text).ToList());
        }

        [Fact]
        public async Task Index_Post_InvaildModel_ReturnsView()
        {
            var model = new PackagingTypesViewModel
            {
                MovementId = movementId
            };

            controller.ModelState.AddModelError("Test", "Error");

            var result = await controller.Index(model);

            Assert.IsAssignableFrom(typeof(ViewResult), result);
            Assert.Equal(string.Empty, ((ViewResult)result).ViewName);
        }

        [Fact]
        public async Task Index_Post_SendsRequestWithCorrectData()
        {
            var selectedPackagingTypes = new[] 
            { 
                new SelectListItem{ Text = "Bag", Value = "5" }, 
                new SelectListItem{ Text = "Box", Value = "4" }
            };
            var packagingTypes = new CheckBoxCollectionViewModel();
            packagingTypes.PossibleValues = selectedPackagingTypes;
            packagingTypes.SetSelectedValues(new[] { 4, 5 });
            var model = new PackagingTypesViewModel
            {
                MovementId = movementId,
                PackagingTypes = packagingTypes
            };

            await controller.Index(model);

            A.CallTo(() => client.SendAsync(A<string>.Ignored,
                A<SetPackagingDataForMovement>.That.Matches(
                    r => r.MovementId == movementId
                    && r.PackagingTypes.Contains(PackagingType.Bag)
                    && r.PackagingTypes.Contains(PackagingType.Box)
                    && r.PackagingTypes.Count == 2)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        private void SetUpPackagingData(IList<PackagingType> availablePackagingTypes, IList<PackagingType> selectedPackagingTypes, string otherDescription = null)
        {
            var packagingData = new PackagingData();

            ObjectInstantiator<PackagingData>.SetProperty<IList<PackagingType>>(
                pd => pd.PackagingTypes,
                availablePackagingTypes,
                packagingData);

            packagingData.OtherDescription = otherDescription;

            if (selectedPackagingTypes != null && selectedPackagingTypes.Any())
            {
                var selectedPackagingData = new PackagingData();

                ObjectInstantiator<PackagingData>.SetProperty<IList<PackagingType>>(
                    pd => pd.PackagingTypes,
                    selectedPackagingTypes,
                    selectedPackagingData);

                A.CallTo(() => client.SendAsync(A<string>.Ignored, A<GetPackagingDataForMovement>.Ignored)).Returns(selectedPackagingData);
            }

            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<GetPackagingDataValidForMovement>.Ignored)).Returns(packagingData);
        }

        private void SetUpPackagingData(IList<PackagingType> packagingTypes, string otherDescription = null)
        {
            SetUpPackagingData(packagingTypes, null, otherDescription);
        }
    }
}
