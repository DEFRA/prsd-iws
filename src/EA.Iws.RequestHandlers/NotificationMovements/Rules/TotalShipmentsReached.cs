namespace EA.Iws.RequestHandlers.NotificationMovements.Rules
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Rules;
    using Domain.Movement;

    internal class TotalShipmentsReached : IMovementRule
    {
        private readonly NumberOfMovements numberOfMovements;

        public TotalShipmentsReached(NumberOfMovements numberOfMovements)
        {
            this.numberOfMovements = numberOfMovements;
        }

        public async Task<RuleResult<MovementRules>> GetResult(Guid notificationId)
        {
            var messageLevel = await numberOfMovements.HasMaximum(notificationId)
                ? MessageLevel.Error
                : MessageLevel.Success;

            return new RuleResult<MovementRules>(MovementRules.TotalShipmentsReached, messageLevel);
        }
    }
}