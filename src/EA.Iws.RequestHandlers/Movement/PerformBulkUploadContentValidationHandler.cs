namespace EA.Iws.RequestHandlers.Movement
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class PerformBulkUploadContentValidationHandler : IRequestHandler<PerformBulkUploadContentValidation, BulkMovementRulesSummary>
    {
        private readonly IMovementRepository repository;

        public PerformBulkUploadContentValidationHandler(IwsContext context, IMovementRepository repository)
        {
            this.repository = repository;
        }

        public async Task<BulkMovementRulesSummary> HandleAsync(PerformBulkUploadContentValidation message)
        {
            // COULLM: Line added to remove warning message. Remove this line at earliest opportunity
            await repository.GetLatestMovementNumber(new System.Guid("87769E5F-99A9-4537-A95C-85C32D875638"));
            
            message.BulkMovementRulesSummary.ContentRulesResults = GetContentRules();
            return message.BulkMovementRulesSummary;
        }

        private List<ContentRuleResult<BulkMovementContentRules>> GetContentRules()
        {
            var rules = new List<ContentRuleResult<BulkMovementContentRules>>();

            rules.Add(new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.MissingData, MessageLevel.Error, new List<string> { "1", "2" }));

            return rules;
        }
    }
}
