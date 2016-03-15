namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class UnitedKingdomCompetentAuthorityMap : IMap<UnitedKingdomCompetentAuthority, UnitedKingdomCompetentAuthorityData>
    {
        private readonly IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMap;
        private readonly IMap<CompetentAuthorityBacsDetails, BacsData> bacsMap;

        public UnitedKingdomCompetentAuthorityMap(IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMap, 
            IMap<CompetentAuthorityBacsDetails, BacsData> bacsMap)
        {
            this.competentAuthorityMap = competentAuthorityMap;
            this.bacsMap = bacsMap;
        }

        public UnitedKingdomCompetentAuthorityData Map(UnitedKingdomCompetentAuthority source)
        {
            return new UnitedKingdomCompetentAuthorityData
            {
                Id = source.Id,
                CompetentAuthority = competentAuthorityMap.Map(source.CompetentAuthority),
                BacsDetails = bacsMap.Map(source.BacsDetails)
            };
        }
    }
}
