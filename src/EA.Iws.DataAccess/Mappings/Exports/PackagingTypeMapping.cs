namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class PackagingTypeMapping : ComplexTypeConfiguration<PackagingType>
    {
        public PackagingTypeMapping()
        {
            Property(x => x.Value).HasColumnName("PackagingType").IsRequired();
            Ignore(x => x.DisplayName);
        }
    }
}