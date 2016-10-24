namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class TechnologyEmployedMapping : EntityTypeConfiguration<TechnologyEmployed>
    {
        public TechnologyEmployedMapping()
        {
            ToTable("TechnologyEmployed", "Notification");

            Property(x => x.NotificationId).HasColumnName("NotificationId").IsRequired();
            Property(x => x.AnnexProvided).HasColumnName("AnnexProvided").IsRequired();
            Property(x => x.Details).HasColumnName("Details").HasMaxLength(70);
            Property(x => x.FurtherDetails).HasColumnName("FurtherDetails");
        }
    }
}
