namespace EA.Iws.DataAccess.Mappings.Common
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class EstimatedValueMapping : ComplexTypeConfiguration<EstimatedValue>
    {
        public EstimatedValueMapping()
        {
            Property(x => x.Amount).HasColumnName("EstimatedAmount").IsRequired();
            Property(x => x.Units).HasColumnName("EstimatedUnit").IsRequired();
        }
    }
}
