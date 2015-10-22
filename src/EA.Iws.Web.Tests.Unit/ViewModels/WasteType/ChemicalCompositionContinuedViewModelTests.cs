namespace EA.Iws.Web.Tests.Unit.ViewModels.WasteType
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Areas.NotificationApplication.ViewModels.ChemicalComposition;
    using Core.WasteType;
    using Requests.WasteType;
    using Xunit;

    public class ChemicalCompositionContinuedViewModelTests
    {
        [Fact]
        public void ValidViewModel_Validates()
        {
            var vm = GetValidViewModel();

            var wasteType = new WasteTypeCompositionData();
            wasteType.MaxConcentration = "78";
            wasteType.MinConcentration = "34";
            vm.WasteComposition = new List<WasteTypeCompositionData>
            {
                wasteType
            };

            wasteType.Constituent = "fudge";
            vm.OtherCodes = new List<WasteTypeCompositionData>
            {
                wasteType
            };

            var result = ValidateViewModel(vm);

            Assert.Empty(result);
        }

        [Fact]
        public void HasAnnexAndNoFurtherInformation_InValid()
        {
            var vm = GetValidViewModel();

            var wasteType = new WasteTypeCompositionData();
            wasteType.MaxConcentration = "78";
            wasteType.MinConcentration = "34";
            vm.WasteComposition = new List<WasteTypeCompositionData>
            {
                wasteType
            };

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
            var wasteType = new WasteTypeCompositionData();
            wasteType.MaxConcentration = maxValue;
            wasteType.MinConcentration = minValue;
            viewModel.WasteComposition = new List<WasteTypeCompositionData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Both fields must either contain 'NA' or a value"));
        }

        [Fact]
        public void AllValues_NA_ValidationError()
        {
            var viewModel = GetValidViewModel();

            var wasteType = new WasteTypeCompositionData();
            wasteType.MaxConcentration = "na";
            wasteType.MinConcentration = "na";
            viewModel.WasteComposition = new List<WasteTypeCompositionData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("You've not entered any data about the waste's composition"));
        }

        [Fact]
        public void WasteComposition_NoMinValues_ValidationError()
        {
            var viewModel = GetValidViewModel();
            var wasteType = new WasteTypeCompositionData();
            wasteType.MaxConcentration = "20";
            viewModel.WasteComposition = new List<WasteTypeCompositionData>
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
            var wasteType = new WasteTypeCompositionData();
            wasteType.MinConcentration = "2";
            viewModel.WasteComposition = new List<WasteTypeCompositionData>
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
            var wasteType = new WasteTypeCompositionData();
            wasteType.MinConcentration = "2";
            wasteType.MaxConcentration = value;
            viewModel.WasteComposition = new List<WasteTypeCompositionData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);
            
            Assert.NotEmpty(result);
            Assert.True(result.Select(r => r.ErrorMessage).Any(e => e.StartsWith("Max concentration should be in range from 0 to 100")));
        }

        [Theory]
        [InlineData("200")]
        [InlineData("-1")]
        public void WasteCodes_MinPercentageNotValid_ValidationError(string value)
        {
            var viewModel = GetValidViewModel();
            var wasteType = new WasteTypeCompositionData();
            wasteType.MinConcentration = value;
            wasteType.MaxConcentration = "7";
            viewModel.WasteComposition = new List<WasteTypeCompositionData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.Select(r => r.ErrorMessage).Any(e => e.StartsWith("Min concentration should be in range from 0 to 100")));
        }

        [Fact]
        public void WasteComposition_MinNotLessThanMax_ValidationError()
        {
            var viewModel = GetValidViewModel();
            var wasteType1 = new WasteTypeCompositionData();
            wasteType1.MinConcentration = "30";
            wasteType1.MaxConcentration = "20";

            var wasteType2 = new WasteTypeCompositionData();
            wasteType2.MinConcentration = "10";
            wasteType2.MaxConcentration = "20";

            viewModel.WasteComposition = new List<WasteTypeCompositionData>
            {
                wasteType1,
                wasteType2
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Min concentration should be lower than the Max concentration"));
        }

        [Fact]
        public void OtherCodes_MinNotLessThanMax_ValidationError()
        {
            var viewModel = GetValidViewModel();
            var wasteType = new WasteTypeCompositionData();
            wasteType.MinConcentration = "30";
            wasteType.MaxConcentration = "20";
            viewModel.OtherCodes = new List<WasteTypeCompositionData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Min concentration should be lower than the Max concentration"));
        }

        [Theory]
        [InlineData("200")]
        [InlineData("-1")]
        public void OtherCodes_MaxPercentageNotValid_ValidationError(string value)
        {
            var viewModel = GetValidViewModel();
            var wasteType = new WasteTypeCompositionData();
            wasteType.MinConcentration = "-2";
            wasteType.MaxConcentration = value;
            viewModel.OtherCodes = new List<WasteTypeCompositionData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
        }

        [Fact]
        public void OtherCodes_NoMaxValues_ValidationError()
        {
            var viewModel = GetValidViewModel();
            var wasteType = new WasteTypeCompositionData();
            wasteType.MinConcentration = "2";
            wasteType.Constituent = "type";
            viewModel.OtherCodes = new List<WasteTypeCompositionData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Please enter a Min and Max concentration for"));
        }

        [Fact]
        public void OtherCodes_NoMinValues_ValidationError()
        {
            var viewModel = GetValidViewModel();
            var wasteType = new WasteTypeCompositionData();
            wasteType.MaxConcentration = "20";
            wasteType.Constituent = "type";
            viewModel.OtherCodes = new List<WasteTypeCompositionData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Please enter a Min and Max concentration for"));
        }

        [Fact]
        public void OtherCodes_NoConstituent_ValidationError()
        {
            var viewModel = GetValidViewModel();
            var wasteType = new WasteTypeCompositionData();
            wasteType.MaxConcentration = "20";
            wasteType.MinConcentration = "10";
            viewModel.OtherCodes = new List<WasteTypeCompositionData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Please enter a name for the 'Other' component"));
        }

        [Theory]
        [InlineData("20", "na")]
        [InlineData("na", "20")]
        public void Other_NaAndNumber_ValidationError(string minValue, string maxValue)
        {
            var viewModel = GetValidViewModel();
            var wasteType = new WasteTypeCompositionData();
            wasteType.MaxConcentration = maxValue;
            wasteType.MinConcentration = minValue;
            wasteType.Constituent = "type";
            viewModel.OtherCodes = new List<WasteTypeCompositionData>
            {
                wasteType
            };

            var result = ValidateViewModel(viewModel);

            Assert.NotEmpty(result);
            Assert.True(result.First().ErrorMessage.StartsWith("Both fields must either contain 'NA' or a value"));
        }

        private static ChemicalCompositionContinuedViewModel GetValidViewModel()
        {
            var vm = new ChemicalCompositionContinuedViewModel();
            vm.WasteComposition = new List<WasteTypeCompositionData>();
            vm.OtherCodes = new List<WasteTypeCompositionData>();
            vm.ChemicalCompositionType = ChemicalCompositionType.Wood;
            vm.Command = "continue";

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
