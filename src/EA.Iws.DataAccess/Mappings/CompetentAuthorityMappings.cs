namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class CompetentAuthorityMappings : EntityTypeConfiguration<CompetentAuthority>
    {
        public CompetentAuthorityMappings()
        {
            ToTable("CompetentAuthority", "Lookup");
        }
    }
}
