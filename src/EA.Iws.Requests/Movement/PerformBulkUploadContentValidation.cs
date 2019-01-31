namespace EA.Iws.Requests.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement.Bulk;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovements)]
    public class PerformBulkUploadContentValidation : IRequest<BulkMovementRulesSummary>
    {
        public BulkMovementRulesSummary BulkMovementRulesSummary { get; private set; }

        public Guid NotificationId { get; private set; }

        public DataTable DataTable { get; private set; }

        public bool IsCsv { get; private set; }

        public PerformBulkUploadContentValidation(BulkMovementRulesSummary bulkMovementRulesSummary, 
            Guid notificationId, 
            DataTable dataTable,
            bool isCsv)
        {
            BulkMovementRulesSummary = bulkMovementRulesSummary;
            NotificationId = notificationId;
            DataTable = dataTable;
            IsCsv = isCsv;
        }
    }
}