namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using Domain;
    using Domain.TransportRoute;
    using FakeItEasy;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class StateOfExportValidatorTests
    {
        private readonly Core.ImportNotification.Draft.StateOfExport stateOfExport = new Core.ImportNotification.Draft.StateOfExport(AnyGuid)
        {
            CountryId = TestableCountry.France.Id,
            CompetentAuthorityId = TestableCompetentAuthority.FrenchAuthorityArdeche.Id,
            ExitPointId = TestableEntryOrExitPoint.Calais.Id
        };

        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly StateOfExportValidator validator;
        private static readonly Guid AnyGuid = new Guid("D8942E55-68D3-4C14-A526-5A53D42E7CBB");

        public StateOfExportValidatorTests()
        {
            entryOrExitPointRepository = A.Fake<IEntryOrExitPointRepository>();
            competentAuthorityRepository = A.Fake<ICompetentAuthorityRepository>();

            validator = new StateOfExportValidator(entryOrExitPointRepository, competentAuthorityRepository);

            A.CallTo(() => entryOrExitPointRepository.GetById(TestableEntryOrExitPoint.Calais.Id))
                .Returns(TestableEntryOrExitPoint.Calais);
            A.CallTo(() => competentAuthorityRepository.GetById(TestableCompetentAuthority.FrenchAuthorityArdeche.Id))
                .Returns(TestableCompetentAuthority.FrenchAuthorityArdeche);
            A.CallTo(() => competentAuthorityRepository.GetById(TestableCompetentAuthority.EnvironmentAgency.Id))
                .Returns(TestableCompetentAuthority.EnvironmentAgency);
            A.CallTo(() => entryOrExitPointRepository.GetById(TestableEntryOrExitPoint.Dover.Id))
                .Returns(TestableEntryOrExitPoint.Dover);
        }

        [Fact]
        public void ValidStateOfExport_ReturnsSuccess()
        {
            var result = validator.Validate(stateOfExport);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void CompetentAuthorityInWrongCountry_ReturnsFailure()
        {
            stateOfExport.CompetentAuthorityId = TestableCompetentAuthority.EnvironmentAgency.Id;

            validator.ShouldHaveValidationErrorFor(x => x.CompetentAuthorityId, stateOfExport);
        }

        [Fact]
        public void ExitPointInWrongCountry_ReturnsFailure()
        {
            stateOfExport.ExitPointId = TestableEntryOrExitPoint.Dover.Id;

            validator.ShouldHaveValidationErrorFor(x => x.ExitPointId, stateOfExport);
        }

        [Fact]
        public void CompetentAuthorityMissing_ReturnsFailure()
        {
            stateOfExport.CompetentAuthorityId = null;

            validator.ShouldHaveValidationErrorFor(x => x.CompetentAuthorityId, stateOfExport);
        }

        [Fact]
        public void CountryMissing_ReturnsFailure()
        {
            stateOfExport.CountryId = null;

            validator.ShouldHaveValidationErrorFor(x => x.CountryId, stateOfExport);
        }

        [Fact]
        public void ExitPointMissing_ReturnsFailure()
        {
            stateOfExport.ExitPointId = null;

            validator.ShouldHaveValidationErrorFor(x => x.ExitPointId, stateOfExport);
        }
    }
}
