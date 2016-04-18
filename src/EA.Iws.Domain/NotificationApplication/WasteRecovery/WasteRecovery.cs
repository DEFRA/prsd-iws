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

        protected WasteRecovery()
        {
        }

        public WasteRecovery(Guid notificationId,
            Percentage percentageRecoverable,
            EstimatedValue estimatedValue,
            RecoveryCost recoveryCost)
        {
            NotificationId = notificationId;
            PercentageRecoverable = percentageRecoverable;
            EstimatedValue = estimatedValue;
            RecoveryCost = recoveryCost;
        }

        public void Update(Percentage percentageRecoverable,
            EstimatedValue estimatedValue,
            RecoveryCost recoveryCost)
        {
            PercentageRecoverable = percentageRecoverable;
            EstimatedValue = estimatedValue;
            RecoveryCost = recoveryCost;

            RaiseEvent(new PercentageChangedEvent(this.NotificationId, percentageRecoverable));
        }
    }
}
