namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shared;
    using Domain;
    using Prsd.Core.Mapper;

    internal class UnitedKingdomCompetentAuthorityMap : IMap<UnitedKingdomCompetentAuthority, UnitedKingdomCompetentAuthorityData>
    {
        private readonly IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMap;
        private readonly IMap<Address, AddressData> addressMap;
        private readonly IMap<CompetentAuthorityBacsDetails, BacsData> bacsMap;

        public UnitedKingdomCompetentAuthorityMap(IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMap, 
            IMap<Address, AddressData> addressMap,
            IMap<CompetentAuthorityBacsDetails, BacsData> bacsMap)
        {
            this.competentAuthorityMap = competentAuthorityMap;
            this.addressMap = addressMap;
            this.bacsMap = bacsMap;
        }

        public UnitedKingdomCompetentAuthorityData Map(UnitedKingdomCompetentAuthority source)
        {
            return new UnitedKingdomCompetentAuthorityData
            {
                Id = source.Id,
                BusinessUnit = source.BusinessUnit,
                Building = source.Building,
                Telephone = source.Telephone,
                CompetentAuthority = competentAuthorityMap.Map(source.CompetentAuthority),
                Address = addressMap.Map(source.Address),
                BacsDetails = bacsMap.Map(source.BacsDetails)
            };
        }
    }
}
