namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class PhysicalCharacteristicsInfoMapping : EntityTypeConfiguration<PhysicalCharacteristicsInfo>
    {
        public PhysicalCharacteristicsInfoMapping()
        {
            ToTable("PhysicalCharacteristicsInfo", "Business");

            Property(x => x.OtherDescription).HasColumnName("OtherDescription").HasMaxLength(120);
        }
    }
}