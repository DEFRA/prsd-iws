namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotificationAssessment.Transactions;

    public class ImportNotificationTransactionMapping : EntityTypeConfiguration<ImportNotificationTransaction>
    {
        public ImportNotificationTransactionMapping()
        {
            ToTable("Transaction", "ImportNotification");
        }
    }
}
