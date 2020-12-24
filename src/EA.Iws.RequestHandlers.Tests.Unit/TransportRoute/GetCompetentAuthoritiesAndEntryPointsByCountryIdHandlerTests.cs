namespace EA.Iws.RequestHandlers.Tests.Unit.TransportRoute
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using Domain.TransportRoute;
    using EA.Iws.Core.Notification;
    using EA.Iws.TestHelpers.DomainFakes;
    using FakeItEasy;
    using RequestHandlers.Mappings;
    using RequestHandlers.TransportRoute;
    using Requests.TransportRoute;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetCompetentAuthoritiesAndEntryPointsByCountryIdHandlerTests
    {
        private readonly Guid countryWithDataId = new Guid("D771AE3E-3196-4D8F-99B5-D423D21501F1");
        private readonly Guid unitedKingdomCountryId = new Guid("F771AE3E-3196-4D8F-99B5-D423D21501FF");
        private readonly Guid countryWithNoDataId = new Guid("4BE0CA01-724B-4F75-B25C-949E1222A93B");
        private readonly CompetentAuthority[] competentAuthorities;
        private readonly GetCompetentAuthoritiesAndEntryPointsByCountryIdHandler handler;
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly IIntraCountryExportAllowedRepository intraCountryExportAllowedRepository;
        private readonly ICompetentAuthorityRepository repository;
        private readonly IEnumerable<Guid> ids;
        private readonly IUnitedKingdomCompetentAuthorityRepository unitedKingdomCompetentAuthorityRepository;

        public GetCompetentAuthoritiesAndEntryPointsByCountryIdHandlerTests()
        {
            entryOrExitPointRepository = A.Fake<IEntryOrExitPointRepository>();
            intraCountryExportAllowedRepository = A.Fake<IIntraCountryExportAllowedRepository>();
            unitedKingdomCompetentAuthorityRepository = A.Fake<IUnitedKingdomCompetentAuthorityRepository>();
            var iwsContext = new TestIwsContext();

            var countryWithData = CountryFactory.Create(countryWithDataId);
            var unitedKingdomCountry = CountryFactory.Create(unitedKingdomCountryId);
            var countryWithNoData = CountryFactory.Create(countryWithNoDataId);
            competentAuthorities = new[]
            {
                CompetentAuthorityFactory.Create(new Guid("67D2B3B5-298A-4BB5-901C-0C0C80097242"), unitedKingdomCountry),
                CompetentAuthorityFactory.Create(new Guid("5E7CA40F-D7B5-49C3-8850-694D36D52C94"), countryWithData),
                CompetentAuthorityFactory.Create(new Guid("DFD98B0D-F255-4BA0-96A5-527DE9F973E3"), countryWithData)
            };

            A.CallTo(() => unitedKingdomCompetentAuthorityRepository.GetAll()).Returns(new[]
            {
                new TestableUnitedKingdomCompetentAuthority(1, competentAuthorities[0], "something", null)
            });

            var competentAuthorityMapper = new CompetentAuthorityMap();
            var entryOrExitPointMapper = new EntryOrExitPointMap();

            A.CallTo(() => entryOrExitPointRepository.GetForCountry(countryWithDataId)).Returns(new[]
            {
                EntryOrExitPointFactory.Create(new Guid("FC012C3E-4252-4D62-A8A2-D46DE0FA93B9"), countryWithData),
                EntryOrExitPointFactory.Create(new Guid("9699CC16-6EF1-4889-9598-F4B0511A2038"), countryWithData)
            });

            A.CallTo(() => intraCountryExportAllowedRepository.GetImportCompetentAuthorities(UKCompetentAuthority.England)).Returns(new[] 
            {
                new TestableIntraCountryExportAllowed
                {
                    ExportCompetentAuthority = UKCompetentAuthority.England,
                    ImportCompetentAuthorityId = competentAuthorities[1].Id
                }
            });

            repository = A.Fake<ICompetentAuthorityRepository>();

            A.CallTo(() => repository.GetCompetentAuthorities(countryWithDataId)).Returns(competentAuthorities);

            ids = new Guid[]
            {
                competentAuthorities[1].Id
            };

            A.CallTo(repository).Where(call => call.Method.Name == "GetByIds")
                                .WithReturnType<CompetentAuthority[]>()
                                .Returns(competentAuthorities);

            handler = new GetCompetentAuthoritiesAndEntryPointsByCountryIdHandler(entryOrExitPointMapper, competentAuthorityMapper, repository, entryOrExitPointRepository, intraCountryExportAllowedRepository, iwsContext, unitedKingdomCompetentAuthorityRepository);
        }

        [Fact]
        public async Task Handle_DataForCountry_NonUk()
        {
            // Arrange
            A.CallTo(() => unitedKingdomCompetentAuthorityRepository.IsCountryUk(countryWithDataId)).Returns(false);

            // Act
            var result = await handler.HandleAsync(new GetCompetentAuthoritiesAndEntryPointsByCountryId(countryWithDataId, UKCompetentAuthority.England));

            // Assert
            A.CallTo(() => unitedKingdomCompetentAuthorityRepository.IsCountryUk(countryWithDataId)).MustHaveHappened();
            A.CallTo(() => entryOrExitPointRepository.GetForCountry(countryWithDataId)).MustHaveHappened();
            A.CallTo(() => repository.GetCompetentAuthorities(countryWithDataId));
        }

        [Fact]
        public async Task Handle_DataForCountry_Uk()
        {
            // Arrange
            A.CallTo(() => unitedKingdomCompetentAuthorityRepository.IsCountryUk(unitedKingdomCountryId)).Returns(true);

            // Act
            var result = await handler.HandleAsync(new GetCompetentAuthoritiesAndEntryPointsByCountryId(unitedKingdomCountryId, UKCompetentAuthority.England));

            // Assert
            A.CallTo(() => unitedKingdomCompetentAuthorityRepository.IsCountryUk(unitedKingdomCountryId)).MustHaveHappened();
            A.CallTo(() => entryOrExitPointRepository.GetForCountry(unitedKingdomCountryId)).MustHaveHappened();
            A.CallTo(() => intraCountryExportAllowedRepository.GetImportCompetentAuthorities(UKCompetentAuthority.England)).MustHaveHappened();
            A.CallTo(repository).Where(call => call.Method.Name == "GetByIds").MustHaveHappened();
        }
    }
}
