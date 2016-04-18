namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Domain.TransportRoute;

    internal class EntryOrExitPointMapping : EntityTypeConfiguration<EntryOrExitPoint>
    {
        public EntryOrExitPointMapping()
        {
            this.ToTable("EntryOrExitPoint", "Notification");

            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(p => p.Name).HasMaxLength(2048);
        }
    }
}
