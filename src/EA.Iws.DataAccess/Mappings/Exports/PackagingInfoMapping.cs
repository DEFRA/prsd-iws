namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class PackagingInfoMapping : EntityTypeConfiguration<PackagingInfo>
    {
        public PackagingInfoMapping()
        {
            ToTable("PackagingInfo", "Notification");

            Property(x => x.OtherDescription).HasColumnName("OtherDescription").HasMaxLength(100);
        }
    }
}