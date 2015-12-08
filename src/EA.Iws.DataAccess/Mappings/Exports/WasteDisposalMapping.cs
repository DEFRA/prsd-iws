namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication.WasteRecovery;

    internal class WasteDisposalMapping : EntityTypeConfiguration<WasteDisposal>
    {
        public WasteDisposalMapping()
        {
            ToTable("DisposalInfo", "Notification");
        }
    }
}
