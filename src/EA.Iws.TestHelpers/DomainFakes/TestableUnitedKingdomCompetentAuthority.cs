namespace EA.Iws.TestHelpers.DomainFakes
{
    using EA.Iws.Domain;

    public class TestableUnitedKingdomCompetentAuthority : UnitedKingdomCompetentAuthority
    {
        public TestableUnitedKingdomCompetentAuthority(int id, 
                                                        CompetentAuthority competentAuthority, 
                                                        string unitedKingdomCountry, 
                                                        CompetentAuthorityBacsDetails bacsDetails)
        {
            this.Id = id;
            this.CompetentAuthority = competentAuthority;
            this.UnitedKingdomCountry = unitedKingdomCountry;
            this.BacsDetails = bacsDetails;
        }
    }
}
