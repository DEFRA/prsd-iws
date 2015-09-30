namespace EA.Iws.Domain.NotificationApplication.Recovery
{
    using System;
    using Prsd.Core.Domain;

    public class RecoveryInfo : Entity
    {
        public Guid NotificationId { get; private set; }
        public Percentage PercentageRecoverable { get; private set; }
        public EstimatedValue EstimatedValue { get; private set; }
        public RecoveryCost RecoveryCost { get; private set; }
        public DisposalCost DisposalCost { get; private set; }

        protected RecoveryInfo()
        {
        }

        public RecoveryInfo(Guid notificationId,
            Percentage percentageRecoverable,
            EstimatedValue estimatedValue,
            RecoveryCost recoveryCost,
            DisposalCost disposalCost)
        {
            NotificationId = notificationId;
            PercentageRecoverable = percentageRecoverable;
            EstimatedValue = estimatedValue;
            RecoveryCost = recoveryCost;
            DisposalCost = disposalCost;
        }

        public void UpdateRecoveryInfo(Percentage percentageRecoverable,
            EstimatedValue estimatedValue,
            RecoveryCost recoveryCost,
            DisposalCost disposalCost)
        {
            PercentageRecoverable = percentageRecoverable;
            EstimatedValue = estimatedValue;
            RecoveryCost = recoveryCost;
            DisposalCost = disposalCost;
        }
    }
}
