namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using EA.Iws.Domain.TransportRoute;
    using EA.Iws.TestHelpers.DomainFakes;
    using Xunit;

    public class TransportRouteValidatorTests
    {
        private readonly Country unitedKingdomCountry = new TestableCountry
        {
            Id = new Guid("BE8189B9-CEAA-45EF-A920-10712FEEFCA6")
        };
        private readonly Country nonUkCountry = new TestableCountry
        {
            Id = new Guid("BE8189B9-CEAA-45EF-A920-10712FEEFCA8")
        };
        private readonly CompetentAuthority firstUkCompetentAuthority = new TestableCompetentAuthority
        {
            Id = new Guid("BE8189B9-CEAA-45EF-A920-10712FEEF456")
        };
        private readonly CompetentAuthority secondUkCompetentAuthority = new TestableCompetentAuthority
        {
            Id = new Guid("BE8189B9-CEAA-45EF-A920-10712FEEF457")
        };
        private readonly CompetentAuthority thirdUkCompetentAuthority = new TestableCompetentAuthority
        {
            Id = new Guid("BE8189B9-CEAA-45EF-A920-10712FEEF458")
        };

        private readonly IEnumerable<IntraCountryExportAllowed> allowedCombinations;
        private readonly IEnumerable<UnitedKingdomCompetentAuthority> unitedKingdomAuthorities;

        public TransportRouteValidatorTests()
        {
            allowedCombinations = new IntraCountryExportAllowed[]
            {
                new TestableIntraCountryExportAllowed
                {
                    ExportCompetentAuthority = Core.Notification.UKCompetentAuthority.Scotland,
                    ImportCompetentAuthorityId = thirdUkCompetentAuthority.Id
                }
            };
            unitedKingdomAuthorities = new UnitedKingdomCompetentAuthority[]
            {
                new TestableUnitedKingdomCompetentAuthority((int)Core.Notification.UKCompetentAuthority.England, firstUkCompetentAuthority, string.Empty, null),
                new TestableUnitedKingdomCompetentAuthority((int)Core.Notification.UKCompetentAuthority.Scotland, secondUkCompetentAuthority, string.Empty, null),
                new TestableUnitedKingdomCompetentAuthority((int)Core.Notification.UKCompetentAuthority.NorthernIreland, thirdUkCompetentAuthority, string.Empty, null),
            };
        }

        [Fact]
        public void IsImportAndExportStatesCombinationValid_OnlyImportState_ReturnsTrue()
        {
            // Arrange
            var validator = new TransportRouteValidation(allowedCombinations, unitedKingdomAuthorities);
            
            // Act
            bool result = validator.IsImportAndExportStatesCombinationValid(new TestableStateOfImport(), null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsImportAndExportStatesCombinationValid_OnlyExportState_ReturnsTrue()
        {
            // Arrange
            var validator = new TransportRouteValidation(allowedCombinations, unitedKingdomAuthorities);

            // Act
            bool result = validator.IsImportAndExportStatesCombinationValid(null, new TestableStateOfExport());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsImportAndExportStatesCombinationValid_DifferentCountries_ReturnsTrue()
        {
            // Arrange
            var export = new TestableStateOfExport
            {
                Country = unitedKingdomCountry
            };
            var import = new TestableStateOfImport
            {
                Country = nonUkCountry
            };
            var validator = new TransportRouteValidation(allowedCombinations, unitedKingdomAuthorities);

            // Act
            bool result = validator.IsImportAndExportStatesCombinationValid(import, export);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsImportAndExportStatesCombinationValid_SameCountryNotAllowed_ReturnsFalse()
        {
            // Arrange
            var export = new TestableStateOfExport
            {
                Country = unitedKingdomCountry,
                CompetentAuthority = firstUkCompetentAuthority
            };
            var import = new TestableStateOfImport
            {
                Country = unitedKingdomCountry,
                CompetentAuthority = secondUkCompetentAuthority
            };
            var validator = new TransportRouteValidation(allowedCombinations, unitedKingdomAuthorities);

            // Act
            bool result = validator.IsImportAndExportStatesCombinationValid(import, export);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsImportAndExportStatesCombinationValid_SameCountryAllowed_ReturnsTrue()
        {
            // Arrange
            var export = new TestableStateOfExport
            {
                Country = unitedKingdomCountry,
                CompetentAuthority = secondUkCompetentAuthority
            };
            var import = new TestableStateOfImport
            {
                Country = unitedKingdomCountry,
                CompetentAuthority = thirdUkCompetentAuthority
            };
            var validator = new TransportRouteValidation(allowedCombinations, unitedKingdomAuthorities);

            // Act
            bool result = validator.IsImportAndExportStatesCombinationValid(import, export);

            // Assert
            Assert.True(result);
        }
    }
}
