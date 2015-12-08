namespace EA.Iws.DataAccess.Mappings.Common
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class UnitedKingdomCompetentAuthorityMapping : EntityTypeConfiguration<UnitedKingdomCompetentAuthority>
    {
        public UnitedKingdomCompetentAuthorityMapping()
        {
            ToTable("UnitedKingdomCompetentAuthority", "Lookup");

            Property(x => x.BusinessUnit).HasMaxLength(4000);
            Property(x => x.Telephone).HasMaxLength(128);
            Ignore(x => x.CountryName);
        }
    }
}
