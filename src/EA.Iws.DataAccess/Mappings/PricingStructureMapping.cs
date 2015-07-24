namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class PricingStructureMapping : EntityTypeConfiguration<PricingStructure>
    {
        public PricingStructureMapping()
        {
            ToTable("PricingStructure", "Lookup");
        }
    }
}