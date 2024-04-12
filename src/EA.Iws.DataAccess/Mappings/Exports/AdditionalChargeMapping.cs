namespace EA.Iws.DataAccess.Mappings.Exports
{
    using EA.Iws.Domain.NotificationApplication;
    using System.Data.Entity.ModelConfiguration;

    internal class AdditionalChargeMapping : EntityTypeConfiguration<AdditionalCharge>
    {
        public AdditionalChargeMapping()
        {
            ToTable("AdditionalCharges", "Notification");
        }
    }
}
