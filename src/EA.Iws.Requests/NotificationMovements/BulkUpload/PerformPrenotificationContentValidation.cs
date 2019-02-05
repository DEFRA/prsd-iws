namespace EA.Iws.Requests.NotificationMovements.BulkUpload
{
    using System;
    using System.Data;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement.BulkPrenotification;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovements)]
    public class PerformPrenotificationContentValidation : IRequest<PrenotificationRulesSummary>
    {
        public PrenotificationRulesSummary RulesSummary { get; private set; }

        public Guid NotificationId { get; private set; }

        public DataTable DataTable { get; private set; }

        public string FileName { get; private set; }

        public bool IsCsv { get; private set; }

        public PerformPrenotificationContentValidation(PrenotificationRulesSummary rulesSummary, 
            Guid notificationId, 
            DataTable dataTable,
            string fileName,
            bool isCsv)
        {
            RulesSummary = rulesSummary;
            NotificationId = notificationId;
            DataTable = dataTable;
            FileName = fileName;
            IsCsv = isCsv;
        }
    }
}