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
                    x.ExportCompetentAuthorityId,
                    x.ImportCompetentAuthorityId
                });
            Property(x => x.ExportCompetentAuthorityId).IsRequired();
            Property(x => x.ImportCompetentAuthorityId).IsRequired();
        }
    }
}