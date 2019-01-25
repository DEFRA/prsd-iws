namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
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
            var shipments = new ContentRulesDTOList();
            message.BulkMovementRulesSummary.ContentRulesResults = await GetContentRules(shipments.ObjectsList);
            return message.BulkMovementRulesSummary;
        }

        private async Task<List<ContentRuleResult<BulkMovementContentRules>>> GetContentRules(List<ContentRulesDTO> shipments)
        {
            var rules = new List<ContentRuleResult<BulkMovementContentRules>>();

            foreach (var rule in contentRules)
            {
                rules.Add(await rule.GetResult(shipments));
            }

            return rules;
        }
    }
}
