namespace EA.Iws.DataAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    public class CompetentAuthorityMapping : EntityTypeConfiguration<CompetentAuthority>
    {
        public CompetentAuthorityMapping()
        {
            this.ToTable("CompetentAuthority", "Lookup");

            this.Property(ca => ca.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(ca => ca.Name).HasMaxLength(1023);

            this.Property(ca => ca.Name).HasMaxLength(63);

            this.Property(ca => ca.Code).HasMaxLength(25);
        }
    }
}
