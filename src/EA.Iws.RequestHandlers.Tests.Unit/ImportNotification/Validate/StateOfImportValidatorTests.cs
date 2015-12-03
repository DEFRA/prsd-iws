namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Domain;
    using Domain.TransportRoute;
    using FakeItEasy;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using TestHelpers.Helpers;
    using Xunit;
    using StateOfImport = Core.ImportNotification.Draft.StateOfImport;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "It's a unit test")]
    public class StateOfImportValidatorTests
    {
        private readonly StateOfImportValidator validator;
        private readonly Guid ukId = new Guid("0C57F6D8-F33D-4B33-8A1E-1CF8453FEEF8");
        private readonly Guid nonUkCaId = new Guid("92D7BAE5-9DCA-4E6B-8A83-CABAFEC5FBE7");
        private readonly Guid nonUkEntryPointId = new Guid("1D3B63AF-EE8E-4F10-B258-AD81EE81FFEF");
        private readonly Guid ukCaId = new Guid("61879A46-8AF2-4AFE-850F-A424080C806C");
        private readonly Guid ukEntryPointId = new Guid("E085D5F9-94ED-434C-8EF3-3D0DEFB64E1C");

        public StateOfImportValidatorTests()
        {
            var countryRepo = A.Fake<ICountryRepository>();
            var caRepo = A.Fake<ICompetentAuthorityRepository>();
            var exitPointRepo = A.Fake<IEntryOrExitPointRepository>();

            var uk = CountryFactory.Create(ukId, "United Kingdom");
            var ukCa = CompetentAuthorityFactory.Create(ukCaId, uk);

            var france = CountryFactory.Create(Guid.NewGuid(), "France");
            var franceCa = CompetentAuthorityFactory.Create(nonUkCaId, france);

            A.CallTo(() => countryRepo.GetUnitedKingdomId()).Returns(ukId);

            A.CallTo(() => caRepo.GetById(ukCaId)).Returns(ukCa);
            A.CallTo(() => caRepo.GetById(nonUkCaId)).Returns(franceCa);

            var dover = EntryOrExitPointFactory.Create(ukEntryPointId, uk);
            var calais = EntryOrExitPointFactory.Create(nonUkEntryPointId, france);

            A.CallTo(() => exitPointRepo.GetById(ukEntryPointId)).Returns(dover);
            A.CallTo(() => exitPointRepo.GetById(nonUkEntryPointId)).Returns(calais);

            validator = new StateOfImportValidator(countryRepo, caRepo, exitPointRepo);
        }

        [Fact]
        public void ValidStateOfImport_ResultIsValid()
        {
            var stateOfImport = GetValidStateOfImport();

            var result = validator.Validate(stateOfImport);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void CompetentAuthorityNull_HasValidationError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.CompetentAuthorityId, null as Guid?);
        }

        [Fact]
        public void EntryPointNull_HasValidationError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.EntryPointId, null as Guid?);
        }

        [Fact]
        public void EntryPointNotInUK_HasValidationError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.EntryPointId, nonUkEntryPointId);
        }

        [Fact]
        public void CompetentAuthorityNotInUK_HasValidationError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.CompetentAuthorityId, nonUkCaId);
        }

        private StateOfImport GetValidStateOfImport()
        {
            return new StateOfImport
            {
                CompetentAuthorityId = ukCaId,
                EntryPointId = ukEntryPointId
            };
        }
    }
}