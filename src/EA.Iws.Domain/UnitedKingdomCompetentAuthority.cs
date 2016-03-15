namespace EA.Iws.Domain
{
    using Core.Notification;
    using NotificationApplication;

    public class UnitedKingdomCompetentAuthority
    {
        public const string CountryName = "United Kingdom";

        protected UnitedKingdomCompetentAuthority()
        {
        }

        public int Id { get; protected set; }

        public virtual CompetentAuthority CompetentAuthority { get; protected set; }

        public string UnitedKingdomCountry { get; protected set; }

        public CompetentAuthorityBacsDetails BacsDetails { get; protected set; }

        public bool Equals(UKCompetentAuthority competentAuthority)
        {
            return (int)competentAuthority == this.Id;
        }

        public UKCompetentAuthority AsCompetentAuthority()
        {
            return (UKCompetentAuthority)this.Id;
        }
    }
}