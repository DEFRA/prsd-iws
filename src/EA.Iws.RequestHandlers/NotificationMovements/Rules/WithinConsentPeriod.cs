namespace EA.Iws.RequestHandlers.NotificationMovements.Rules
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Rules;
    using Domain.Movement;

    internal class WithinConsentPeriod : IMovementRule
    {
        private readonly ConsentPeriod consentPeriod;

        public WithinConsentPeriod(ConsentPeriod consentPeriod)
        {
            this.consentPeriod = consentPeriod;
        }

        public async Task<RuleResult<MovementRules>> GetResult(Guid notificationId)
        {
            var messageLevel = await consentPeriod.HasExpired(notificationId)
                ? MessageLevel.Error
                : MessageLevel.Success;

            return new RuleResult<MovementRules>(MovementRules.ConsentPeriodExpired, messageLevel);
        }
    }
}
