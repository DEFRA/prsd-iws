namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using Core.Shared;
    using Prsd.Core.Domain;

    public class RecoveryInfo : Entity
    {
        public Guid NotificationId { get; private set; }
        public EstimatedValue EstimatedValue { get; private set; }
        public RecoveryCost RecoveryCost { get; private set; }
        public DisposalCost DisposalCost { get; private set; }

        protected RecoveryInfo()
        {
        }

        public RecoveryInfo(Guid notificationId,
            EstimatedValue estimatedValue,
            RecoveryCost recoveryCost,
            DisposalCost disposalCost)
        {
            NotificationId = notificationId;
            UpdateEstimatedValue(estimatedValue);
            UpdateRecoveryCost(recoveryCost);
            UpdateDisposalCost(disposalCost);
        }

        public void UpdateEstimatedValue(EstimatedValue estimatedValue)
        {
            EstimatedValue = estimatedValue;
        }

        public void UpdateRecoveryCost(RecoveryCost recoveryCost)
        {
            RecoveryCost = recoveryCost;
        }

        public void UpdateDisposalCost(DisposalCost disposalCost)
        {
            if (disposalCost == null)
            {
                DisposalCost = new DisposalCost();
            }
            else
            {
                DisposalCost = disposalCost;
            }
        }
    }
}
