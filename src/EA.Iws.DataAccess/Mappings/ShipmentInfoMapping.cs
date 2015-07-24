namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    public class ShipmentInfoMapping : EntityTypeConfiguration<ShipmentInfo>
    {
        public ShipmentInfoMapping()
        {
            ToTable("ShipmentInfo", "Business");
        }
    }
}