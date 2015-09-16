namespace EA.Iws.Web.Tests.Unit.ViewModels.WasteType
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Areas.NotificationApplication.ViewModels.WasteType;
    using Core.WasteType;
    using Xunit;

    public class ChemicalCompositionInformationViewModelTests
    {
        [Fact]
        public void ValidViewModel_Validates()
        {
            var vm = GetValidViewModel();

            var wasteType = new WoodInformationData();
            wasteType.MaxConcentration = "78";
            wasteType.MinConcentration = "34";
            vm.WasteComposition = new List<WoodInformationData>
            {
                wasteType
            };

            var result = ValidateViewModel(vm);

            Assert.Empty(result);
        }

        [Fact]
        public void WoodAndNoEnergy_InValid()
        {
            var vm = GetValidViewModel();
            vm.ChemicalCompositionType = ChemicalCompositionType.RDF;

            var result = ValidateViewModel(vm);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Please enter a value for Energy"));
        }

        [Fact]
        public void HasAnnexAndNoFurtherInformation_InValid()
        {
            var vm = GetValidViewModel();
            vm.HasAnnex = true;
            vm.FurtherInformation = "something";

            var result = ValidateViewModel(vm);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("If you select that you are providing the details in a separate annex do not enter any details here"));
        }

        [Theory]
        [InlineData("20", "na")]
        [InlineData("na", "20")]
        public void WasteComposition_NaAndNumber_ValidationError(string minValue, string maxValue)
        {
            var viewModel = GetValidViewModel();
            var wasteType = new WoodInformationData();
            wasteType.MaxConcentration = maxValue;
            wasteType.MinConcentration = minValue;
            viewModel.WasteComposition = new List<WoodInformationData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Both fields must either contain 'NA' or a number"));
        }

        [Fact]
        public void AllValues_NA_ValidationError()
        {
            var viewModel = GetValidViewModel();

            var wasteType = new WoodInformationData();
            wasteType.MaxConcentration = "na";
            wasteType.MinConcentration = "na";
            viewModel.WasteComposition = new List<WoodInformationData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("You’ve not entered any data about the waste’s composition"));
        }

        [Fact]
        public void WasteComposition_NoMinValues_ValidationError()
        {
            var viewModel = GetValidViewModel();
            var wasteType = new WoodInformationData();
            wasteType.MaxConcentration = "20";
            viewModel.WasteComposition = new List<WoodInformationData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Please enter a Min and Max concentration for"));
        }

        [Fact]
        public void WasteComposition_NoMaxValues_ValidationError()
        {
            var viewModel = GetValidViewModel();
            var wasteType = new WoodInformationData();
            wasteType.MinConcentration = "2";
            viewModel.WasteComposition = new List<WoodInformationData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Please enter a Min and Max concentration for"));
        }

        [Theory]
        [InlineData("200")]
        [InlineData("-1")]
        public void WasteCodes_MaxPercentageNotValid_ValidationError(string value)
        {
            var viewModel = GetValidViewModel();
            var wasteType = new WoodInformationData();
            wasteType.MinConcentration = "2";
            wasteType.MaxConcentration = value;
            viewModel.WasteComposition = new List<WoodInformationData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Max concentration should be in range from 0 to 100"));
        }

        [Theory]
        [InlineData("200")]
        [InlineData("-1")]
        public void WasteCodes_MinPercentageNotValid_ValidationError(string value)
        {
            var viewModel = GetValidViewModel();
            var wasteType = new WoodInformationData();
            wasteType.MinConcentration = value;
            wasteType.MaxConcentration = "7";
            viewModel.WasteComposition = new List<WoodInformationData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Min concentration should be in range from 0 to 100"));
        }

        [Fact]
        public void WasteComposition_MinNotLessThanMax_ValidationError()
        {
            var viewModel = GetValidViewModel();
            var wasteType1 = new WoodInformationData();
            wasteType1.MinConcentration = "30";
            wasteType1.MaxConcentration = "20";

            var wasteType2 = new WoodInformationData();
            wasteType2.MinConcentration = "10";
            wasteType2.MaxConcentration = "20";

            viewModel.WasteComposition = new List<WoodInformationData>
            {
                wasteType1,
                wasteType2
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Min concentration should be lower than the Max concentration"));
        }

        private static ChemicalCompositionInformationViewModel GetValidViewModel()
        {
            var vm = new ChemicalCompositionInformationViewModel();
            vm.WasteComposition = new List<WoodInformationData>();
            vm.ChemicalCompositionType = ChemicalCompositionType.Wood;

            return vm;
        }

        private static IEnumerable<ValidationResult> ValidateViewModel(object viewModel)
        {
            var validationContext = new ValidationContext(viewModel, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(viewModel, validationContext, validationResults, false);

            return validationResults;
        }
    }
}
