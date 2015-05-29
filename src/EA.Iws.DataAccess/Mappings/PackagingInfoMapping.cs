namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;

    internal class PackagingInfoMapping : EntityTypeConfiguration<PackagingInfo>
    {
        public PackagingInfoMapping()
        {
            ToTable("PackagingInfo", "Business");

            Property(x => x.OtherDescription).HasColumnName("OtherDescription");
        }
    }
}