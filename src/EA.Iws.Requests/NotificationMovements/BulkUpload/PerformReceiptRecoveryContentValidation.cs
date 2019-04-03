namespace EA.Iws.Requests.NotificationMovements.BulkUpload
{
    using System;
    using System.Data;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement.BulkReceiptRecovery;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovements)]
    public class PerformReceiptRecoveryContentValidation : IRequest<ReceiptRecoveryRulesSummary>
    {
        public ReceiptRecoveryRulesSummary BulkMovementRulesSummary { get; private set; }

        public Guid NotificationId { get; private set; }

        public DataTable DataTable { get; private set; }

        public string FileName { get; private set; }

        public bool IsCsv { get; private set; }

        public PerformReceiptRecoveryContentValidation(ReceiptRecoveryRulesSummary bulkMovementRulesSummary, 
            Guid notificationId, 
            DataTable dataTable,
            string fileName,
            bool isCsv)
        {
            BulkMovementRulesSummary = bulkMovementRulesSummary;
            NotificationId = notificationId;
            DataTable = dataTable;
            FileName = fileName;
            IsCsv = isCsv;
        }
    }
}