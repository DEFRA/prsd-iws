namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class PerformBulkUploadContentValidationHandler : IRequestHandler<PerformBulkUploadContentValidation, BulkMovementRulesSummary>
    {
        private readonly IEnumerable<IBulkMovementPrenotificationContentRule> contentRules;

        public PerformBulkUploadContentValidationHandler(IEnumerable<IBulkMovementPrenotificationContentRule> contentRules)
        {
            this.contentRules = contentRules;
        }

        public async Task<BulkMovementRulesSummary> HandleAsync(PerformBulkUploadContentValidation message)
        {
            var shipments = new PrenotificationMovementCollection();
            message.BulkMovementRulesSummary.ContentRulesResults = await GetContentRules(shipments.ObjectsList, message.NotificationId);
            return message.BulkMovementRulesSummary;
        }

        private async Task<List<ContentRuleResult<BulkMovementContentRules>>> GetContentRules(List<PrenotificationMovement> shipments, Guid notificationId)
        {
            var rules = new List<ContentRuleResult<BulkMovementContentRules>>();

            foreach (var rule in contentRules)
            {
                rules.Add(await rule.GetResult(shipments, notificationId));
            }

            return rules;
        }
    }
}
