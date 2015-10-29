namespace EA.Iws.RequestHandlers.Tests.Unit.TransportRoute
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Domain.TransportRoute;
    using FakeItEasy;
    using RequestHandlers.Mappings;
    using RequestHandlers.TransportRoute;
    using Requests.TransportRoute;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetCompetentAuthoritiesAndEntryOrExitPointsByCountryIdHandlerTests
    {
        private readonly Guid countryWithDataId = new Guid("A62BD60E-9B81-4B8A-B59C-2B4579FF97E7");
        private readonly Guid countryWithNoDataId = new Guid("F6BB7204-3526-4920-80A6-3D31419C94AA");
        private readonly GetCompetentAuthoritiesAndEntryOrExitPointsByCountryIdHandler handler;

        public GetCompetentAuthoritiesAndEntryOrExitPointsByCountryIdHandlerTests()
        {
            var entryOrExitPointRepository = A.Fake<IEntryOrExitPointRepository>();

            var competentAuthorityMapper = new CompetentAuthorityMap();
            var entryOrExitPointMapper = new EntryOrExitPointMap();

            var countryWithData = CountryFactory.Create(countryWithDataId);
            var countryWithNoData = CountryFactory.Create(countryWithNoDataId);

            var competentAuthorities = new[]
            {
                CompetentAuthorityFactory.Create(new Guid("C74A75B9-C338-4330-A43A-3AAF3B8FA5E7"), countryWithData),
                CompetentAuthorityFactory.Create(new Guid("B076AF57-83EB-4F99-BDE6-859CC2B35FBE"), countryWithData)
            };

            A.CallTo(() => entryOrExitPointRepository.GetForCountry(countryWithDataId)).Returns(new[]
            {
                EntryOrExitPointFactory.Create(new Guid("B054CA23-18D3-4E4E-A2B1-F92B7503919A"), countryWithData),
                EntryOrExitPointFactory.Create(new Guid("9F0DC969-8224-4FEC-BD0D-B90B70378323"), countryWithData)
            });

            var repository = A.Fake<ICompetentAuthorityRepository>();
            A.CallTo(() => repository.GetCompetentAuthorities(countryWithDataId)).Returns(competentAuthorities);
            
            handler = new GetCompetentAuthoritiesAndEntryOrExitPointsByCountryIdHandler(entryOrExitPointMapper, competentAuthorityMapper, repository, entryOrExitPointRepository);
        }

        [Fact]
        public async Task Handle_NoDataForCountry_ReturnsEmptyObject()
        {
            var result = await handler.HandleAsync(new GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId(countryWithNoDataId));

            Assert.Empty(result.CompetentAuthorities);
            Assert.Empty(result.EntryOrExitPoints);
        }

        [Fact]
        public async Task Handle_DataForCountry_ReturnsObjectWithData()
        {
            var result = await handler.HandleAsync(new GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId(countryWithDataId));

            Assert.Equal(2, result.CompetentAuthorities.Length);
            Assert.Equal(2, result.EntryOrExitPoints.Length);
        }
    }
}
