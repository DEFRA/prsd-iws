namespace EA.Iws.Web.Tests.Unit.ViewModels.WasteType
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Areas.NotificationApplication.ViewModels.ChemicalComposition;
    using Core.WasteType;
    using Xunit;

    public class ChemicalCompositionViewModelTests
    {
        [Fact]
        public void ValidViewModel_Validates()
        {
            var viewModel = GetValidViewModel();

            var otherType = new WoodInformationData();
            otherType.MaxConcentration = "20";
            otherType.MinConcentration = "3";
            otherType.Constituent = "type";

            var wasteType = new WoodInformationData();
            wasteType.MaxConcentration = "20";
            wasteType.MinConcentration = "3";
            viewModel.WasteComposition = new List<WoodInformationData>
            {
                wasteType
            };

            viewModel.Energy = "energy";

            var result = ValidateViewModel(viewModel);

            Assert.Empty(result);
        }

        [Fact]
        public void WoodWithoutDescription_ValidationError()
        {
            var viewModel = GetValidViewModel();
            viewModel.ChemicalCompositionType = ChemicalCompositionType.Wood;

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.Equals("Description is required"));
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
        public void WoodWithDescription_Validates()
        {
            var viewModel = GetValidViewModel();
            viewModel.ChemicalCompositionType = ChemicalCompositionType.Wood;
            viewModel.Description = "description";

            var wasteType = new WoodInformationData();
            wasteType.MinConcentration = "30";
            wasteType.MaxConcentration = "50";
            wasteType.Constituent = "type";
            viewModel.WasteComposition = new List<WoodInformationData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);

            Assert.Empty(result);
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

            viewModel.Energy = "energy";

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Min concentration should be lower than the Max concentration"));
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

            viewModel.Energy = "energy";

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Max concentration should be in range from 0 to 100"));
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

            viewModel.Energy = "energy";

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Please enter a Min and Max concentration for"));
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

            viewModel.Energy = "energy";

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Please enter a Min and Max concentration for"));
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

            viewModel.Energy = "energy";

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

            viewModel.Energy = "energy";

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("You have not entered any data about the waste's composition"));
        }

        private static ChemicalCompositionViewModel GetValidViewModel()
        {
            var vm = new ChemicalCompositionViewModel();
            vm.WasteComposition = new List<WoodInformationData>();

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
