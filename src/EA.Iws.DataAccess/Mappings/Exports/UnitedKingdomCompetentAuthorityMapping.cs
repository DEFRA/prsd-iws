namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class UnitedKingdomCompetentAuthorityMapping : EntityTypeConfiguration<UnitedKingdomCompetentAuthority>
    {
        public UnitedKingdomCompetentAuthorityMapping()
        {
            ToTable("UnitedKingdomCompetentAuthority", "Lookup");
        }
    }
}
