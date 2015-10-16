namespace EA.Iws.Domain
{
    using CompetentAuthorityEnum = Core.Notification.CompetentAuthority;

    public class UnitedKingdomCompetentAuthority
    {
        protected UnitedKingdomCompetentAuthority()
        {
        }

        public int Id { get; protected set; }

        public virtual CompetentAuthority CompetentAuthority { get; protected set; }

        public string CountryName
        {
            get { return Address.Country; }
        }

        public string UnitedKingdomCountry { get; protected set; }

        public string BusinessUnit { get; protected set; }

        public string Building { get; protected set; }

        public Address Address { get; protected set; }

        public string Telephone { get; protected set; }

        public CompetentAuthorityBacsDetails BacsDetails { get; protected set; }

        public bool Equals(CompetentAuthorityEnum competentAuthority)
        {
            return (int)competentAuthority == this.Id;
        }

        public CompetentAuthorityEnum AsCompetentAuthority()
        {
            return (CompetentAuthorityEnum)this.Id;
        }
    }
}