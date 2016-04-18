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

    public class TransitStateValidatorTests
    {
        private readonly Core.ImportNotification.Draft.TransitState transitState = new Core.ImportNotification.Draft.TransitState(AnyGuid)
        {
            CountryId = TestableCountry.France.Id,
            CompetentAuthorityId = TestableCompetentAuthority.FrenchAuthorityArdeche.Id,
            ExitPointId = TestableEntryOrExitPoint.Calais.Id,
            EntryPointId = TestableEntryOrExitPoint.Cherbourg.Id
        };

        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly TransitStateValidator validator;
        private static readonly Guid AnyGuid = new Guid("38C5C07C-EDC2-4749-9B87-2FDC0330CAE6");

        public TransitStateValidatorTests()
        {
            entryOrExitPointRepository = A.Fake<IEntryOrExitPointRepository>();
            competentAuthorityRepository = A.Fake<ICompetentAuthorityRepository>();

            validator = new TransitStateValidator(entryOrExitPointRepository, competentAuthorityRepository);

            A.CallTo(() => entryOrExitPointRepository.GetById(TestableEntryOrExitPoint.Calais.Id))
                .Returns(TestableEntryOrExitPoint.Calais);
            A.CallTo(() => entryOrExitPointRepository.GetById(TestableEntryOrExitPoint.Cherbourg.Id))
                .Returns(TestableEntryOrExitPoint.Cherbourg);
            A.CallTo(() => competentAuthorityRepository.GetById(TestableCompetentAuthority.FrenchAuthorityArdeche.Id))
                .Returns(TestableCompetentAuthority.FrenchAuthorityArdeche);
            A.CallTo(() => competentAuthorityRepository.GetById(TestableCompetentAuthority.EnvironmentAgency.Id))
                .Returns(TestableCompetentAuthority.EnvironmentAgency);
            A.CallTo(() => entryOrExitPointRepository.GetById(TestableEntryOrExitPoint.Dover.Id))
                .Returns(TestableEntryOrExitPoint.Dover);
        }

        [Fact]
        public void ValidTransitState_ReturnsSuccess()
        {
            var result = validator.Validate(transitState);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void CompetentAuthorityInWrongCountry_ReturnsFailure()
        {
            transitState.CompetentAuthorityId = TestableCompetentAuthority.EnvironmentAgency.Id;

            validator.ShouldHaveValidationErrorFor(x => x.CompetentAuthorityId, transitState);
        }

        [Fact]
        public void ExitPointInWrongCountry_ReturnsFailure()
        {
            transitState.ExitPointId = TestableEntryOrExitPoint.Dover.Id;

            validator.ShouldHaveValidationErrorFor(x => x.ExitPointId, transitState);
        }

        [Fact]
        public void EntryPointInWrongCountry_ReturnsFailure()
        {
            transitState.EntryPointId = TestableEntryOrExitPoint.Dover.Id;

            validator.ShouldHaveValidationErrorFor(x => x.EntryPointId, transitState);
        }

        [Fact]
        public void CompetentAuthorityMissing_ReturnsFailure()
        {
            transitState.CompetentAuthorityId = null;

            validator.ShouldHaveValidationErrorFor(x => x.CompetentAuthorityId, transitState);
        }

        [Fact]
        public void CountryMissing_ReturnsFailure()
        {
            transitState.CountryId = null;

            validator.ShouldHaveValidationErrorFor(x => x.CountryId, transitState);
        }

        [Fact]
        public void ExitPointMissing_ReturnsFailure()
        {
            transitState.ExitPointId = null;

            validator.ShouldHaveValidationErrorFor(x => x.ExitPointId, transitState);
        }

        [Fact]
        public void EntryPointMissing_ReturnsFailure()
        {
            transitState.EntryPointId = null;

            validator.ShouldHaveValidationErrorFor(x => x.EntryPointId, transitState);
        }
    }
}
