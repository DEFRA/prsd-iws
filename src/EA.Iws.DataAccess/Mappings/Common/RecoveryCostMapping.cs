namespace EA.Iws.DataAccess.Mappings.Common
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class RecoveryCostMapping : ComplexTypeConfiguration<RecoveryCost>
    {
        public RecoveryCostMapping()
        {
            Property(x => x.Amount).HasColumnName("CostAmount").IsRequired();
            Property(x => x.Units).HasColumnName("CostUnit").IsRequired();
        }
    }
}
