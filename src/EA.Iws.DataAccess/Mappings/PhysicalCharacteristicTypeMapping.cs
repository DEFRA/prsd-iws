namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class PhysicalCharacteristicTypeMapping : ComplexTypeConfiguration<PhysicalCharacteristicType>
    {
        public PhysicalCharacteristicTypeMapping()
        {
            Property(x => x.Value).HasColumnName("PhysicalCharacteristicType").IsRequired();
            Ignore(x => x.DisplayName);
        }
    }
}