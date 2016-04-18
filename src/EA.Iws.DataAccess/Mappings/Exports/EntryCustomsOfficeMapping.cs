namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.TransportRoute;

    internal class EntryCustomsOfficeMapping : EntityTypeConfiguration<EntryCustomsOffice>
    {
        public EntryCustomsOfficeMapping()
        {
            this.ToTable("EntryCustomsOffice", "Notification");

            this.Property(x => x.Name).HasMaxLength(1042);
            this.Property(x => x.Address).HasMaxLength(4000);
        }
    }
}
