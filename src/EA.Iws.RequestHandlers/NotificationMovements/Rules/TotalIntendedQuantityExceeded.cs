namespace EA.Iws.RequestHandlers.NotificationMovements.Rules
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Rules;
    using Domain.Movement;

    internal class TotalIntendedQuantityExceeded : IMovementRule
    {
        private readonly NotificationMovementsQuantity movementsQuantity;

        public TotalIntendedQuantityExceeded(NotificationMovementsQuantity movementsQuantity)
        {
            this.movementsQuantity = movementsQuantity;
        }

        public async Task<RuleResult<MovementRules>> GetResult(Guid notificationId)
        {
            var messageLevel = await movementsQuantity.Remaining(notificationId) < 0
                ? MessageLevel.Error
                : MessageLevel.Success;

            return new RuleResult<MovementRules>(MovementRules.TotalIntendedQuantityExceeded, messageLevel);
        }
    }
}