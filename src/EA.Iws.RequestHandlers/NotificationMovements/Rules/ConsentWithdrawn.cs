namespace EA.Iws.RequestHandlers.NotificationMovements.Rules
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.NotificationAssessment;
    using Core.Rules;
    using Domain.NotificationAssessment;

    internal class ConsentWithdrawn : IMovementRule
    {
        private readonly INotificationAssessmentRepository notificationApplicationRepository;

        public ConsentWithdrawn(INotificationAssessmentRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<RuleResult<MovementRules>> GetResult(Guid notificationId)
        {
            var status = (await notificationApplicationRepository.GetByNotificationId(notificationId)).Status;
            var messageLevel = status == NotificationStatus.ConsentWithdrawn ? MessageLevel.Error : MessageLevel.Success;

            return new RuleResult<MovementRules>(MovementRules.ConsentWithdrawn, messageLevel);
        }
    }
}
