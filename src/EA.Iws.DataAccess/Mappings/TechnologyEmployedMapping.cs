namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;

    internal class TechnologyEmployedMapping : EntityTypeConfiguration<TechnologyEmployed>
    {
        public TechnologyEmployedMapping()
        {
            ToTable("TechnologyEmployed", "Notification");

            Property(x => x.AnnexProvided).HasColumnName("AnnexProvided").IsRequired();
            Property(x => x.Details).HasColumnName("Details");
        }
    }
}
