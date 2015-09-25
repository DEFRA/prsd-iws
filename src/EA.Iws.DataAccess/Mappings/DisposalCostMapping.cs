namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class DisposalCostMapping : ComplexTypeConfiguration<DisposalCost>
    {
        public DisposalCostMapping()
        {
            Property(x => x.Amount).HasColumnName("DisposalAmount").IsOptional();
            Property(x => x.Units).HasColumnName("DisposalUnit").IsOptional();
        }
    }
}
