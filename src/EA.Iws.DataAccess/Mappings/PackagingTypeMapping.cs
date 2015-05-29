namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class PackagingTypeMapping : ComplexTypeConfiguration<PackagingType>
    {
        public PackagingTypeMapping()
        {
            Property(x => x.Value).HasColumnName("PackagingType").IsRequired();
            Ignore(x => x.DisplayName);
        }
    }
}