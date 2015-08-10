namespace EA.Iws.Core.Shared
{
    public class UnitedKingdomCompetentAuthorityData
    {
        public int Id { get; set; }

        public CompetentAuthorityData CompetentAuthority { get; set; }

        public string CountryName
        {
            get { return Address.CountryName; }
        }

        public string BusinessUnit { get; set; }

        public AddressData Address { get; set; }

        public string Telephone { get; set; }

        public BacsData BacsDetails { get; set; }
    }
}
