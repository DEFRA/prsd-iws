namespace EA.Iws.DocumentGeneration.Tests.Unit.Formatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteType;
    using DocumentGeneration.Formatters;
    using DocumentGeneration.ViewModels;
    using Domain.NotificationApplication;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class WasteCompositionFormatterTests
    {
        private readonly WasteCompositionFormatter formatter
            = new WasteCompositionFormatter();
        private readonly TestableWasteType wasteType = new TestableWasteType();

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        private void EnergyIsNullOrEmpty_ReturnsEmptyString(string energy)
        {
            wasteType.EnergyInformation = energy;

            var result = formatter.GetEnergyEfficiencyString(wasteType);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void EnergyIsSetToString_ReturnsStringWithTitle()
        {
            wasteType.EnergyInformation = "100";

            var result = formatter.GetEnergyEfficiencyString(wasteType);

            Assert.Equal("\r\nEnergy Efficiency = 100", result);
        }

        [Fact]
        public void WasteTypeIsNull_ReturnsEmptyEnergyString()
        {
            var result = formatter.GetEnergyEfficiencyString(null);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void WasteTypeIsNull_ReturnsEmptyOptionalInformationTitle()
        {
            var result = formatter.GetOptionalInformationTitle(null);

            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        private void OptionalInformationIsNull_ReturnsEmptyOptionalInformationTitle(string optionalInformation)
        {
            wasteType.OptionalInformation = optionalInformation;

            var result = formatter.GetOptionalInformationTitle(wasteType);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetWasteCompositionPercentages_ListIsNull_ReturnsEmptyArray()
        {
            wasteType.WasteCompositions = null;

            var result = formatter.GetWasteCompositionPercentages(wasteType);

            Assert.Empty(result);
        }

        [Fact]
        public void GetWasteCompositionPercentages_ListIsEmpty_ReturnsEmptyArray()
        {
            wasteType.WasteCompositions = new List<WasteComposition>();

            var result = formatter.GetWasteCompositionPercentages(wasteType);

            Assert.Empty(result);
        }

        [Theory]
        [InlineData(null, 1, 10, ChemicalCompositionType.SRF)]
        [InlineData("toast", 1, 10, ChemicalCompositionType.SRF)]
        [InlineData("A Chemical Thing", 1.3, 10, ChemicalCompositionType.Other)]
        public void GetWasteCompositionPercentages_ListWithOneItem_ReturnsExpectedResult(string constituent,
            decimal min,
            decimal max,
            ChemicalCompositionCategory category)
        {
            wasteType.WasteCompositions = new List<WasteComposition>
            {
                WasteComposition.CreateWasteComposition(constituent, min, max, category)
            };

            var result = formatter.GetWasteCompositionPercentages(wasteType);

            AssertExpectedCompositionPercentage(constituent, max, min, result.Single());
        }

        [Fact]
        public void GetWasteCompositionPercentages_DoesNotReturnWasteWherePercentagesAreBothZero()
        {
            var wcs = new List<WasteComposition>();
            var wc1 = WasteComposition.CreateWasteComposition("One", 1, 10, ChemicalCompositionCategory.Food);
            var wc2 = WasteComposition.CreateWasteComposition("One", 0, 0, ChemicalCompositionCategory.Food);

            wcs.Add(wc1);
            wcs.Add(wc2);

            wasteType.WasteCompositions = wcs;

            var result = formatter.GetWasteCompositionPercentages(wasteType);

            AssertExpectedCompositionPercentage("One", 10, 1, result.Single());
        }

        [Theory]
        [InlineData(ChemicalCompositionType.RDF, "Refuse Derived Fuel (RDF)")]
        [InlineData(ChemicalCompositionType.SRF, "Solid Recovered Fuel (SRF)")]
        [InlineData(ChemicalCompositionType.Wood, "Wood")]
        public void GetWasteName(ChemicalCompositionType type, string expected)
        {
            var chemicalComposition = ChemicalComposition.RDF;

            if (type == ChemicalCompositionType.SRF)
            {
                chemicalComposition = ChemicalComposition.SRF;
            }
            else if (type == ChemicalCompositionType.Wood)
            {
                chemicalComposition = ChemicalComposition.Wood;
            }

            wasteType.ChemicalCompositionType = chemicalComposition;

            var result = formatter.GetWasteName(wasteType);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetWasteName_TypeOther_ReturnsDescription()
        {
            wasteType.ChemicalCompositionType = ChemicalComposition.Other;
            wasteType.ChemicalCompositionName = "broccoli";

            var result = formatter.GetWasteName(wasteType);

            Assert.Equal(wasteType.ChemicalCompositionName, result);
        }

        [Fact]
        public void GetWasteName_WasteTypeNull_ReturnsEmptyString()
        {
            var result = formatter.GetWasteName(null);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void WasteAdditionalInformationIsNull_ReturnsEmptyArray()
        {
            var result = formatter.GetAdditionalInformationChemicalCompositionPercentages(null);

            Assert.Empty(result);
        }

        [Theory]
        [InlineData("broccoli", 5, 7, WasteInformationType.Chlorine, true)]
        [InlineData("silence", 5.15, 1064, WasteInformationType.HeavyMetals, false)]
        [InlineData("bricks", 0, 1064, WasteInformationType.NetCalorificValue, false)]
        public void GetWasteAdditionalInformationCompositonPercentages_ReturnExpectedResult(string constituent,
            decimal min,
            decimal max,
            WasteInformationType type,
            bool includeUnits)
        {
            wasteType.WasteAdditionalInformation = new List<WasteAdditionalInformation>
            {
                WasteAdditionalInformation.CreateWasteAdditionalInformation(constituent,
                min, max, type)
            };

            var result =
                formatter.GetAdditionalInformationChemicalCompositionPercentages(wasteType.WasteAdditionalInformation);
            
            AssertExpectedCompositionPercentage(constituent, max, min, result.Single(), includeUnits);
        }

        private void AssertExpectedCompositionPercentage(string constituent, 
            decimal max, 
            decimal min,
            ChemicalCompositionPercentages result,
            bool includeUnits = true)
        {
            var name = string.Empty;

            if (constituent != null)
            {
                name = includeUnits ? constituent + " wt/wt %" : constituent;
            }

            Assert.Equal(name, result.Name);
            Assert.Equal(max.ToString("N"), result.Max);
            Assert.Equal(min.ToString("N"), result.Min);
        }
    }
}
