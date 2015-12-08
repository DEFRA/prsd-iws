namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.TransportRoute;

    internal class ExitCustomsOfficeMapping : EntityTypeConfiguration<ExitCustomsOffice>
    {
        public ExitCustomsOfficeMapping()
        {
            this.ToTable("ExitCustomsOffice", "Notification");

            this.Property(x => x.Name).HasMaxLength(1042);
            this.Property(x => x.Address).HasMaxLength(4000);
        }
    }
}
