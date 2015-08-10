namespace EA.Iws.RequestHandlers.Submit
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Shared;

    internal class GetUnitedKingdomCompetentAuthorityInformationHandler : IRequestHandler<GetUnitedKingdomCompetentAuthorityInformation, UnitedKingdomCompetentAuthorityData>
    {
        private readonly IwsContext context;
        private readonly IMap<UnitedKingdomCompetentAuthority, UnitedKingdomCompetentAuthorityData> unitedKingdomCompetentAuthorityMap;

        public GetUnitedKingdomCompetentAuthorityInformationHandler(IwsContext context,
            IMap<UnitedKingdomCompetentAuthority, UnitedKingdomCompetentAuthorityData> unitedKingdomCompetentAuthorityMap)
        {
            this.context = context;
            this.unitedKingdomCompetentAuthorityMap = unitedKingdomCompetentAuthorityMap;
        }

        public async Task<UnitedKingdomCompetentAuthorityData> HandleAsync(GetUnitedKingdomCompetentAuthorityInformation message)
        {
            var result =
                await
                    context.UnitedKingdomCompetentAuthorities.SingleAsync(ca => ca.Id == (int)message.CompetentAuthority);

            return unitedKingdomCompetentAuthorityMap.Map(result);
        }
    }
}
