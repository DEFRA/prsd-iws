namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class PerformBulkUploadContentValidationHandler : IRequestHandler<PerformBulkUploadContentValidation, BulkMovementRulesSummary>
    {
        private readonly IEnumerable<IBulkMovementPrenotificationContentRule> contentRules;
        private readonly IMap<DataTable, List<PrenotificationMovement>> mapper;

        public PerformBulkUploadContentValidationHandler(IEnumerable<IBulkMovementPrenotificationContentRule> contentRules,
            IMap<DataTable, List<PrenotificationMovement>> mapper)
        {
            this.contentRules = contentRules;
            this.mapper = mapper;
        }

        public async Task<BulkMovementRulesSummary> HandleAsync(PerformBulkUploadContentValidation message)
        {
            var result = message.BulkMovementRulesSummary;

            result.PrenotificationMovements = mapper.Map(message.DataTable);

            result.ContentRulesResults = await GetContentRules(result.PrenotificationMovements, message.NotificationId);

            return result;
        }

        private async Task<List<ContentRuleResult<BulkMovementContentRules>>> GetContentRules(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var rules = new List<ContentRuleResult<BulkMovementContentRules>>();

            foreach (var rule in contentRules)
            {
                rules.Add(await rule.GetResult(movements, notificationId));
            }

            return rules;
        }
    }
}
