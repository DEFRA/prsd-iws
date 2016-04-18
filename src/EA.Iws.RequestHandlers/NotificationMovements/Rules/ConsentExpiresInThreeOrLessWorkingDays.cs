namespace EA.Iws.RequestHandlers.NotificationMovements.Rules
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Rules;
    using Domain.Movement;

    internal class ConsentExpiresInThreeOrLessWorkingDays : IMovementRule
    {
        private readonly ConsentPeriod consentPeriod;

        public ConsentExpiresInThreeOrLessWorkingDays(ConsentPeriod consentPeriod)
        {
            this.consentPeriod = consentPeriod;
        }

        public async Task<RuleResult<MovementRules>> GetResult(Guid notificationId)
        {
            var messageLevel = await consentPeriod.ExpiresInThreeOrLessWorkingDays(notificationId)
                ? MessageLevel.Error
                : MessageLevel.Success;

            return new RuleResult<MovementRules>(MovementRules.ConsentExpiresInThreeOrLessWorkingDays, messageLevel);
        }
    }
}
