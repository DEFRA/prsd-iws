namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication.Annexes;

    internal class ProcessOfGenerationAnnexMapping : ComplexTypeConfiguration<ProcessOfGenerationAnnex>
    {
        public ProcessOfGenerationAnnexMapping()
        {
            Property(x => x.FileId).HasColumnName("ProcessOfGenerationId");
        }
    }
}
