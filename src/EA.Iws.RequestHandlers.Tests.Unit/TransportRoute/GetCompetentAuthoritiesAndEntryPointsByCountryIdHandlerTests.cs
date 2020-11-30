namespace EA.Iws.RequestHandlers.Tests.Unit.TransportRoute
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Domain.TransportRoute;
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
        private readonly Guid countryWithNoDataId = new Guid("4BE0CA01-724B-4F75-B25C-949E1222A93B");

        private readonly CompetentAuthority[] competentAuthorities;

        private readonly GetCompetentAuthoritiesAndEntryPointsByCountryIdHandler handler;

        public GetCompetentAuthoritiesAndEntryPointsByCountryIdHandlerTests()
        {
            var entryOrExitPointRepository = A.Fake<IEntryOrExitPointRepository>();
            var intraCountryExportAllowedRepository = A.Fake<IIntraCountryExportAllowedRepository>();

            var competentAuthorityMapper = new CompetentAuthorityMap();
            var entryOrExitPointMapper = new EntryOrExitPointMap();

            var countryWithData = CountryFactory.Create(countryWithDataId);
            var countryWithNoData = CountryFactory.Create(countryWithNoDataId);

            competentAuthorities = new[]
            {
                CompetentAuthorityFactory.Create(new Guid("67D2B3B5-298A-4BB5-901C-0C0C80097242"), countryWithData),
                CompetentAuthorityFactory.Create(new Guid("5E7CA40F-D7B5-49C3-8850-694D36D52C94"), countryWithData),
                CompetentAuthorityFactory.Create(new Guid("DFD98B0D-F255-4BA0-96A5-527DE9F973E3"), countryWithData)
            };

            A.CallTo(() => entryOrExitPointRepository.GetForCountry(countryWithDataId)).Returns(new[]
            {
                EntryOrExitPointFactory.Create(new Guid("FC012C3E-4252-4D62-A8A2-D46DE0FA93B9"), countryWithData),
                EntryOrExitPointFactory.Create(new Guid("9699CC16-6EF1-4889-9598-F4B0511A2038"), countryWithData)
            });

            A.CallTo(() => intraCountryExportAllowedRepository.GetImportCompetentAuthorities(competentAuthorities[0].Id)).Returns(new[] { new TestableIntraCountryExportAllowed { ExportCompetentAuthorityId = competentAuthorities[0].Id, ImportCompetentAuthorityId = competentAuthorities[1].Id } });
            A.CallTo(() => intraCountryExportAllowedRepository.GetImportCompetentAuthorities(competentAuthorities[1].Id)).Returns(new[] { new TestableIntraCountryExportAllowed { ExportCompetentAuthorityId = competentAuthorities[1].Id, ImportCompetentAuthorityId = competentAuthorities[2].Id } });
            A.CallTo(() => intraCountryExportAllowedRepository.GetImportCompetentAuthorities(competentAuthorities[2].Id)).Returns(new[] { new TestableIntraCountryExportAllowed { ExportCompetentAuthorityId = competentAuthorities[2].Id, ImportCompetentAuthorityId = competentAuthorities[1].Id }, new TestableIntraCountryExportAllowed { ExportCompetentAuthorityId = competentAuthorities[2].Id, ImportCompetentAuthorityId = competentAuthorities[0].Id } });
            A.CallTo(() => intraCountryExportAllowedRepository.GetAll()).Returns(new[]
            {
                new TestableIntraCountryExportAllowed { ExportCompetentAuthorityId = competentAuthorities[0].Id, ImportCompetentAuthorityId = competentAuthorities[1].Id },
                new TestableIntraCountryExportAllowed { ExportCompetentAuthorityId = competentAuthorities[2].Id, ImportCompetentAuthorityId = competentAuthorities[1].Id },
                new TestableIntraCountryExportAllowed { ExportCompetentAuthorityId = competentAuthorities[2].Id, ImportCompetentAuthorityId = competentAuthorities[0].Id },
                new TestableIntraCountryExportAllowed { ExportCompetentAuthorityId = competentAuthorities[1].Id, ImportCompetentAuthorityId = competentAuthorities[2].Id }
            });

            var repository = A.Fake<ICompetentAuthorityRepository>();
            A.CallTo(() => repository.GetCompetentAuthorities(countryWithDataId)).Returns(competentAuthorities);

            handler = new GetCompetentAuthoritiesAndEntryPointsByCountryIdHandler(entryOrExitPointMapper, competentAuthorityMapper, repository, entryOrExitPointRepository, intraCountryExportAllowedRepository);
        }

        [Fact]
        public async Task Handle_NoDataForCountry_ReturnsEmptyObject()
        {
            var result = await handler.HandleAsync(new GetCompetentAuthoritiesAndEntryPointsByCountryId(countryWithNoDataId, null));

            Assert.Empty(result.CompetentAuthorities);
            Assert.Empty(result.EntryOrExitPoints);
        }

        [Fact]
        public async Task Handle_DataForCountry_ReturnsObjectWithData()
        {
            var result = await handler.HandleAsync(new GetCompetentAuthoritiesAndEntryPointsByCountryId(countryWithDataId, null));

            Assert.Equal(3, result.CompetentAuthorities.Length);
            Assert.Equal(2, result.EntryOrExitPoints.Length);
        }

        [Fact]
        public async Task Handle_DataForCountryWhenExitPointSpecified_ReturnsDataLimitedByImportCA()
        {
            var result = await handler.HandleAsync(new GetCompetentAuthoritiesAndEntryPointsByCountryId(countryWithDataId, competentAuthorities[1].Id));

            Assert.Single(result.CompetentAuthorities);
            Assert.Equal(2, result.EntryOrExitPoints.Length);

            Assert.Equal(competentAuthorities[2].Id, result.CompetentAuthorities[0].Id);
        }
    }
}
