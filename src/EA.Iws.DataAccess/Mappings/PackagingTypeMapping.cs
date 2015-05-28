namespace EA.Iws.DataAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class PackagingTypeMapping : EntityTypeConfiguration<PackagingType>
    {
        public PackagingTypeMapping()
        {
            ToTable("ShippingInfoPackagingType", "Business");

            Property(x => x.Value).HasColumnName("PackagingType").IsRequired();
            Ignore(x => x.DisplayName);
            HasKey(x => x.Id);
        }
    }
}