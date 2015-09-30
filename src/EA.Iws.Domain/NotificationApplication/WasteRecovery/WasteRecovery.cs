namespace EA.Iws.Domain.NotificationApplication.WasteRecovery
{
    using System;
    using Prsd.Core.Domain;

    public class WasteRecovery : Entity
    {
        public Guid NotificationId { get; private set; }
        public Percentage PercentageRecoverable { get; private set; }
        public EstimatedValue EstimatedValue { get; private set; }
        public RecoveryCost RecoveryCost { get; private set; }
        public DisposalCost DisposalCost { get; private set; }

        protected WasteRecovery()
        {
        }

        public WasteRecovery(Guid notificationId,
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

        public void UpdateWasteRecovery(Percentage percentageRecoverable,
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
