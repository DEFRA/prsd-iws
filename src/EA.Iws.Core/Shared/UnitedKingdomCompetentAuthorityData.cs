namespace EA.Iws.Core.Shared
{
    using Notification;

    public class UnitedKingdomCompetentAuthorityData
    {
        public int Id { get; set; }

        public CompetentAuthorityData CompetentAuthority { get; set; }

        public BacsData BacsDetails { get; set; }

        public UKCompetentAuthority AsUKCompetantAuthority()
        {
            return (UKCompetentAuthority)Id;
        }
    }
}
