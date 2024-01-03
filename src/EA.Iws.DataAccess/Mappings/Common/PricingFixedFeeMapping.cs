namespace EA.Iws.DataAccess.Mappings.Common
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Finance;

    internal class PricingFixedFeeMapping : EntityTypeConfiguration<PricingFixedFee>
    {
        public PricingFixedFeeMapping()
        {
            ToTable("PricingFixedFee", "Lookup");
        }
    }
}