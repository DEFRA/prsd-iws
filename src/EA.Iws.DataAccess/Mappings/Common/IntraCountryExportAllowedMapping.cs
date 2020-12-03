namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using EA.Iws.Domain;

    public class IntraCountryExportAllowedMapping : EntityTypeConfiguration<IntraCountryExportAllowed>
    {
        public IntraCountryExportAllowedMapping()
        {
            ToTable("IntraCountryExportAllowed", "Lookup")
                .HasKey(x => new
                {
                    x.ExportCompetentAuthority,
                    x.ImportCompetentAuthorityId
                });
            Property(x => x.ExportCompetentAuthority).IsRequired();
            Property(x => x.ImportCompetentAuthorityId).IsRequired();
        }
    }
}