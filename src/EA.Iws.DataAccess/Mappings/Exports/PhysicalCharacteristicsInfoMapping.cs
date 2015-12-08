namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class PhysicalCharacteristicsInfoMapping : EntityTypeConfiguration<PhysicalCharacteristicsInfo>
    {
        public PhysicalCharacteristicsInfoMapping()
        {
            ToTable("PhysicalCharacteristicsInfo", "Notification");

            Property(x => x.OtherDescription).HasColumnName("OtherDescription").HasMaxLength(120);
        }
    }
}