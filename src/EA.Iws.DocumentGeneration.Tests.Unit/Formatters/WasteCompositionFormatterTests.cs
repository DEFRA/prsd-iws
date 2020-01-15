﻿namespace EA.Iws.DocumentGeneration.Tests.Unit.Formatters
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
        [InlineData(null, 1, 10, ChemicalCompositionCategory.Food)]
        [InlineData("toast", 1, 10, ChemicalCompositionCategory.Food)]
        [InlineData("A Chemical Thing", 1.3, 10, ChemicalCompositionCategory.Other)]
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
        public void GetAdditionalInformationCompositionPercentages_DoesNotReturnWasteWherePercentagesAreBothZero()
        {
            var wcs = new List<WasteAdditionalInformation>();
            var wa1 = WasteAdditionalInformation.CreateWasteAdditionalInformation("One", 1, 10, WasteInformationType.AshContent);
            var wa2 = WasteAdditionalInformation.CreateWasteAdditionalInformation("One", 0, 0, WasteInformationType.MoistureContent);

            wcs.Add(wa1);
            wcs.Add(wa2);

            var result = formatter.GetAdditionalInformationChemicalCompositionPercentages(wcs);

            AssertExpectedCompositionPercentage("One", 10, 1, result.Single());
        }

        [Fact]
        public void GetAdditionalInformationCompositionPercentages_DoesReturnWasteWherePercentagesHasOneZero()
        {
            var wcs = new List<WasteAdditionalInformation>();
            var wa = WasteAdditionalInformation.CreateWasteAdditionalInformation("One", 0, 10, WasteInformationType.AshContent);

            wcs.Add(wa);

            var result = formatter.GetAdditionalInformationChemicalCompositionPercentages(wcs);

            AssertExpectedCompositionPercentage("One", 10, 0, result.Single());
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

        [Fact]
        public void GetWasteCompositionPercentages_DoesReturnWasteWherePercentagesHasOneZero()
        {
            var wcs = new List<WasteComposition>();
            var wc = WasteComposition.CreateWasteComposition("One", 0, 10, ChemicalCompositionCategory.Food);

            wcs.Add(wc);

            wasteType.WasteCompositions = wcs;

            var result = formatter.GetWasteCompositionPercentages(wasteType);

            AssertExpectedCompositionPercentage("One", 10, 0, result.Single());
        }

        [Theory]
        [InlineData(ChemicalComposition.RDF, "Refuse Derived Fuel (RDF)")]
        [InlineData(ChemicalComposition.SRF, "Solid Recovered Fuel (SRF)")]
        [InlineData(ChemicalComposition.Wood, "Wood")]
        public void GetWasteName(ChemicalComposition type, string expected)
        {
            var chemicalComposition = ChemicalComposition.RDF;

            if (type == ChemicalComposition.SRF)
            {
                chemicalComposition = ChemicalComposition.SRF;
            }
            else if (type == ChemicalComposition.Wood)
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
        [InlineData("broccoli", 5, 7, WasteInformationType.Chlorine)]
        [InlineData("silence", 5.15, 1064, WasteInformationType.HeavyMetals)]
        [InlineData("bricks", 0, 1064, WasteInformationType.NetCalorificValue)]
        public void GetWasteAdditionalInformationCompositonPercentages_ReturnExpectedResult(string constituent,
            decimal min,
            decimal max,
            WasteInformationType type)
        {
            wasteType.WasteAdditionalInformation = new List<WasteAdditionalInformation>
            {
                WasteAdditionalInformation.CreateWasteAdditionalInformation(constituent,
                min, max, type)
            };

            var result =
                formatter.GetAdditionalInformationChemicalCompositionPercentages(wasteType.WasteAdditionalInformation);

            AssertExpectedCompositionPercentageUnits(constituent, max, min, type, result.Single());
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

        private void AssertExpectedCompositionPercentageUnits(string constituent,
            decimal max,
            decimal min,
            WasteInformationType wasteInformationType,
            ChemicalCompositionPercentages result)
        {
            var name = string.Empty;

            if (wasteInformationType == WasteInformationType.HeavyMetals ||
                wasteInformationType == WasteInformationType.NetCalorificValue)
            {
                name = wasteInformationType == WasteInformationType.HeavyMetals
                    ? constituent + " mg/kg"
                    : constituent + " MJ/kg";
            }
            else
            {
                name = constituent + " wt/wt %";
            }

            Assert.Equal(name, result.Name);
            Assert.Equal(max.ToString("N"), result.Max);
            Assert.Equal(min.ToString("N"), result.Min);
        }
    }
}
