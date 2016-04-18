namespace EA.Iws.DataAccess.Mappings.Common
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Finance;

    internal class PricingStructureMapping : EntityTypeConfiguration<PricingStructure>
    {
        public PricingStructureMapping()
        {
            ToTable("PricingStructure", "Lookup");
        }
    }
}