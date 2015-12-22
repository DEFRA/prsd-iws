namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication.Annexes;

    internal class TechnologyEmployedAnnexMapping : ComplexTypeConfiguration<TechnologyEmployedAnnex>
    {
        public TechnologyEmployedAnnexMapping()
        {
            Property(x => x.FileId).HasColumnName("TechnologyEmployedId");
        }
    }
}
