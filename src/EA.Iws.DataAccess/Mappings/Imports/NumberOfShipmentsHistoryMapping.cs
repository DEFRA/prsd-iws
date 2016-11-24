namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;

    public class NumberOfShipmentsHistoryMapping : EntityTypeConfiguration<NumberOfShipmentsHistory>
    {
        public NumberOfShipmentsHistoryMapping()
        {
            ToTable("NumberOfShipmentsHistory", "ImportNotification");
            
            Property(x => x.NumberOfShipments).IsRequired();
            Property(x => x.ImportNotificationId).IsRequired();
            Property(x => x.DateChanged).IsRequired();
        }
    }
}
