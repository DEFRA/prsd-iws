namespace EA.Iws.RequestHandlers.NotificationMovements.Rules
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Rules;
    using Domain;
    using Domain.Movement;

    internal class TotalIntendedQuantityReached : IMovementRule
    {
        private readonly NotificationMovementsQuantity movementsQuantity;

        public TotalIntendedQuantityReached(NotificationMovementsQuantity movementsQuantity)
        {
            this.movementsQuantity = movementsQuantity;
        }

        public async Task<RuleResult<MovementRules>> GetResult(Guid notificationId)
        {
            var remaining = await movementsQuantity.Remaining(notificationId);
            var messageLevel = remaining == new ShipmentQuantity(0, remaining.Units)
                ? MessageLevel.Error
                : MessageLevel.Success;

            return new RuleResult<MovementRules>(MovementRules.TotalIntendedQuantityReached, messageLevel);
        }
    }
}