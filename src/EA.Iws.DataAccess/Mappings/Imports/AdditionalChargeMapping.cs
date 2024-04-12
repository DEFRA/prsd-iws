namespace EA.Iws.DataAccess.Mappings.Imports
{
    using EA.Iws.Domain.ImportNotification;
    using System.Data.Entity.ModelConfiguration;

    internal class AdditionalChargeMapping : EntityTypeConfiguration<AdditionalCharge>
    {
        public AdditionalChargeMapping()
        {
            ToTable("AdditionalCharges", "ImportNotification");
        }
    }
}
