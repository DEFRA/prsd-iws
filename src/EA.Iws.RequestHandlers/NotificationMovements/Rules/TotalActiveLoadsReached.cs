namespace EA.Iws.RequestHandlers.NotificationMovements.Rules
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Rules;
    using Domain.Movement;

    internal class TotalActiveLoadsReached : IMovementRule
    {
        private readonly NumberOfActiveLoads numberOfActiveLoads;

        public TotalActiveLoadsReached(NumberOfActiveLoads numberOfActiveLoads)
        {
            this.numberOfActiveLoads = numberOfActiveLoads;
        }

        public async Task<RuleResult<MovementRules>> GetResult(Guid notificationId)
        {
            var messageLevel = await numberOfActiveLoads.HasMaximum(notificationId) ? MessageLevel.Error : MessageLevel.Success;

            return new RuleResult<MovementRules>(MovementRules.ActiveLoadsReached, messageLevel);
        }
    }
}
