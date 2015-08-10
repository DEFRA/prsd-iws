namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using FakeItEasy;
    using Helpers;
    using Mappings;
    using Prsd.Core.Mapper;
    using Requests.Shared;
    using Submit;
    using Xunit;
    using CompetentAuthority = Core.Notification.CompetentAuthority;

    public class GetUnitedKingdomCompetentAuthorityInformationHandlerTests
    {
        private readonly GetUnitedKingdomCompetentAuthorityInformationHandler handler;
        private readonly IMap<CompetentAuthorityBacsDetails, BacsData> bacsMap; 

        public GetUnitedKingdomCompetentAuthorityInformationHandlerTests()
        {
            var context = A.Fake<IwsContext>();

            var competentAuthorityMap = A.Fake<IMap<Domain.CompetentAuthority, CompetentAuthorityData>>();
            A.CallTo(() => competentAuthorityMap.Map(A<Domain.CompetentAuthority>.Ignored)).Returns(null);

            var addressMap = A.Fake<IMap<Address, AddressData>>();
            A.CallTo(() => addressMap.Map(A<Address>.Ignored)).Returns(null);

            bacsMap = A.Fake<IMap<CompetentAuthorityBacsDetails, BacsData>>();
            A.CallTo(() => bacsMap.Map(A<CompetentAuthorityBacsDetails>.Ignored)).Returns(null);
            
            handler = new GetUnitedKingdomCompetentAuthorityInformationHandler(context, 
                new UnitedKingdomCompetentAuthorityMap(competentAuthorityMap, 
                addressMap, 
                bacsMap));

            var helper = new DbContextHelper();

            A.CallTo(() => context.UnitedKingdomCompetentAuthorities).Returns(helper.GetAsyncEnabledDbSet(new List<UnitedKingdomCompetentAuthority>
            {
                new TestUnitedKingdomCompetentAuthority((int)CompetentAuthority.England),
                new TestUnitedKingdomCompetentAuthority((int)CompetentAuthority.Scotland) 
            }));
        }

        [Fact]
        public async Task ReturnsCompetentAuthorityWithCorrectId()
        {
            var result = await handler.HandleAsync(new GetUnitedKingdomCompetentAuthorityInformation(CompetentAuthority.England));

            Assert.Equal((int)CompetentAuthority.England, result.Id);
        }

        [Fact]
        public async Task MapsBacsDetails()
        {
            var result =
                await
                    handler.HandleAsync(new GetUnitedKingdomCompetentAuthorityInformation(CompetentAuthority.Scotland));

            A.CallTo(() => bacsMap.Map(A<CompetentAuthorityBacsDetails>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }

        private class TestUnitedKingdomCompetentAuthority : UnitedKingdomCompetentAuthority
        {
            public new int Id
            {
                get { return base.Id; }
                set { base.Id = value; }
            }

            public TestUnitedKingdomCompetentAuthority(int id)
            {
                base.Id = id;
            }
        }
    }
}
