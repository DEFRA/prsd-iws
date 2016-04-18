namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class OperationCodesEntityMapping : EntityTypeConfiguration<OperationInfo>
    {
        public OperationCodesEntityMapping()
        {
            ToTable("OperationCodes", "Notification");
        }
    }
}
