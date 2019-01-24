namespace EA.Iws.Requests.Movement
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement.Bulk;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovements)]
    public class PerformBulkUploadContentValidation : IRequest<BulkMovementRulesSummary>
    {
        public BulkMovementRulesSummary BulkMovementRulesSummary { get; private set; }

        public PerformBulkUploadContentValidation(BulkMovementRulesSummary bulkMovementRulesSummary)
        {
            BulkMovementRulesSummary = bulkMovementRulesSummary;
        }
    }
}