namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class DisposalCostMapping : ComplexTypeConfiguration<DisposalCost>
    {
        public DisposalCostMapping()
        {
            Property(x => x.Amount).HasColumnName("Amount").IsRequired();
            Property(x => x.Units).HasColumnName("Unit").IsRequired();
        }
    }
}
