namespace EA.Iws.RequestHandlers.NotificationMovements.Rules
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Rules;
    using Domain.Movement;

    internal class ConsentExpiresInFourWorkingDays : IMovementRule
    {
        private readonly ConsentPeriod consentPeriod;

        public ConsentExpiresInFourWorkingDays(ConsentPeriod consentPeriod)
        {
            this.consentPeriod = consentPeriod;
        }

        public async Task<RuleResult<MovementRules>> GetResult(Guid notificationId)
        {
            var messageLevel = await consentPeriod.ExpiresInFourWorkingDays(notificationId)
                ? MessageLevel.Error
                : MessageLevel.Success;

            return new RuleResult<MovementRules>(MovementRules.ConsentExpiresInFourWorkingDays, messageLevel);
        }
    }
}
