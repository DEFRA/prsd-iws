namespace EA.Iws.RequestHandlers.NotificationMovements
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Rules;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;

    internal class GetMovementRulesSummaryHandler : IRequestHandler<GetMovementRulesSummary, MovementRulesSummary>
    {
        private readonly IEnumerable<IMovementRule> movementRules;

        public GetMovementRulesSummaryHandler(IEnumerable<IMovementRule> movementRules)
        {
            this.movementRules = movementRules;
        }

        public async Task<MovementRulesSummary> HandleAsync(GetMovementRulesSummary message)
        {
            var rules = new List<RuleResult<MovementRules>>();

            foreach (var rule in movementRules)
            {
                rules.Add(await rule.GetResult(message.NotificationId));
            }

            return new MovementRulesSummary(rules);
        }
    }
}