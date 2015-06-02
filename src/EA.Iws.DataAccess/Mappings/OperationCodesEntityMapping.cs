namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;

    internal class OperationCodesEntityMapping : EntityTypeConfiguration<OperationInfo>
    {
        public OperationCodesEntityMapping()
        {
            ToTable("OperationCodes", "Business");
        }
    }
}
