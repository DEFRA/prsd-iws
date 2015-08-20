namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain;
    using FakeItEasy;
    using Mappings;
    using Prsd.Core.Mapper;
    using RequestHandlers.Mappings;
    using Requests.Shared;
    using Submit;
    using Xunit;

    public class GetUnitedKingdomCompetentAuthorityInformationHandlerTests
    {
        private readonly GetUnitedKingdomCompetentAuthorityInformationHandler handler;
        private readonly IMap<CompetentAuthorityBacsDetails, BacsData> bacsMap; 

        public GetUnitedKingdomCompetentAuthorityInformationHandlerTests()
        {
            var context = new TestIwsContext();

            var competentAuthorityMap = A.Fake<IMap<CompetentAuthority, CompetentAuthorityData>>();
            A.CallTo(() => competentAuthorityMap.Map(A<CompetentAuthority>.Ignored)).Returns(null);

            var addressMap = A.Fake<IMap<Address, AddressData>>();
            A.CallTo(() => addressMap.Map(A<Address>.Ignored)).Returns(null);

            bacsMap = A.Fake<IMap<CompetentAuthorityBacsDetails, BacsData>>();
            A.CallTo(() => bacsMap.Map(A<CompetentAuthorityBacsDetails>.Ignored)).Returns(null);
            
            handler = new GetUnitedKingdomCompetentAuthorityInformationHandler(context, 
                new UnitedKingdomCompetentAuthorityMap(competentAuthorityMap, 
                addressMap, 
                bacsMap));

            context.UnitedKingdomCompetentAuthorities.AddRange(new List<UnitedKingdomCompetentAuthority>
            {
                new TestUnitedKingdomCompetentAuthority((int)Core.Notification.CompetentAuthority.England),
                new TestUnitedKingdomCompetentAuthority((int)Core.Notification.CompetentAuthority.Scotland) 
            });
        }

        [Fact]
        public async Task ReturnsCompetentAuthorityWithCorrectId()
        {
            var result = await handler.HandleAsync(new GetUnitedKingdomCompetentAuthorityInformation(Core.Notification.CompetentAuthority.England));

            Assert.Equal((int)Core.Notification.CompetentAuthority.England, result.Id);
        }

        [Fact]
        public async Task MapsBacsDetails()
        {
            var result =
                await
                    handler.HandleAsync(new GetUnitedKingdomCompetentAuthorityInformation(Core.Notification.CompetentAuthority.Scotland));

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
