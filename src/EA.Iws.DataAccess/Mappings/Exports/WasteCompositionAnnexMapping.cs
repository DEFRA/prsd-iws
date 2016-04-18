namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication.Annexes;

    internal class WasteCompositionAnnexMapping : ComplexTypeConfiguration<WasteCompositionAnnex>
    {
        public WasteCompositionAnnexMapping()
        {
            Property(x => x.FileId).HasColumnName("WasteCompositionId");
        }
    }
}
