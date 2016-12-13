namespace EA.Iws.RequestHandlers.NotificationMovements.Rules
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Rules;
    using Domain.FinancialGuarantee;

    internal class ActiveApprovedFinancialGuarantee : IMovementRule
    {
        private readonly IFinancialGuaranteeRepository repository;

        public ActiveApprovedFinancialGuarantee(IFinancialGuaranteeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<RuleResult<MovementRules>> GetResult(Guid notificationId)
        {
            var financialGuaranteeCollection = await repository.GetByNotificationId(notificationId);

            var financialGuarantee = financialGuaranteeCollection.GetCurrentApprovedFinancialGuarantee();

            var messageLevel = financialGuarantee == null ? MessageLevel.Error : MessageLevel.Success;

            return new RuleResult<MovementRules>(MovementRules.HasApprovedFinancialGuarantee, messageLevel);
        }
    }
}