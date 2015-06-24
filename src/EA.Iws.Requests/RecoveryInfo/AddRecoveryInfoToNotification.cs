namespace EA.Iws.Requests.RecoveryInfo
{
    using System;
    using Core.RecoveryInfo;
    using Prsd.Core.Mediator;

    public class AddRecoveryInfoToNotification : IRequest<Guid>
    {
        public RecoveryInfoUnits EstimatedUnit { get; set; }

        public RecoveryInfoUnits CostUnit { get; set; }

        public RecoveryInfoUnits? DisposalUnit { get; set; }

        public decimal EstimatedAmount { get; set; }

        public decimal CostAmount { get; set; }

        public decimal? DisposalAmount { get; set; }

        public Guid NotificationId { get; set; }

        public bool IsDisposal { get; set; }
        
        public AddRecoveryInfoToNotification(Guid notificationId, bool isDisposal, 
            RecoveryInfoUnits estimatedUnit, decimal estimatedAmount,
            RecoveryInfoUnits costUnit, decimal costAmount,
            RecoveryInfoUnits? disposalUnit, decimal? disposalAmount)
        {
            NotificationId = notificationId;
            IsDisposal = isDisposal;
            EstimatedUnit = estimatedUnit;
            EstimatedAmount = estimatedAmount;
            CostUnit = costUnit;
            CostAmount = costAmount;
            DisposalUnit = disposalUnit.GetValueOrDefault();
            DisposalAmount = disposalAmount.GetValueOrDefault();
        }
    }
}