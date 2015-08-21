namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement;

    public class MovementMapping : EntityTypeConfiguration<Movement>
    {
        public MovementMapping()
        {
            ToTable("Movement", "Notification");

            Property(x => x.NotificationApplicationId).HasColumnName("NotificationId");
        }
    }
}
