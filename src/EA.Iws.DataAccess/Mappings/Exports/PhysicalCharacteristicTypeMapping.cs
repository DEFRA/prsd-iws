namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class PhysicalCharacteristicTypeMapping : ComplexTypeConfiguration<PhysicalCharacteristicType>
    {
        public PhysicalCharacteristicTypeMapping()
        {
            Property(x => x.Value).HasColumnName("PhysicalCharacteristicType").IsRequired();
            Ignore(x => x.DisplayName);
        }
    }
}