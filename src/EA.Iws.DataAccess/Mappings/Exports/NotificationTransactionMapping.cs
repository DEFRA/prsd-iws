namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationAssessment;

    internal class NotificationTransactionMapping : EntityTypeConfiguration<NotificationTransaction>
    {
        public NotificationTransactionMapping()
        {
            ToTable("Transaction", "Notification");

            Property(x => x.Comments).HasMaxLength(500);

            Property(x => x.ReceiptNumber).HasMaxLength(100);

            Property(x => x.Date).IsRequired();

            Property(x => x.NotificationId).IsRequired();
        }
    }
}
