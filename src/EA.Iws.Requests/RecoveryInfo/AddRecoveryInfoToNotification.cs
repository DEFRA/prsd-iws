namespace EA.Iws.Requests.RecoveryInfo
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    public class AddRecoveryInfoToNotification : IRequest<Guid>
    {
        public ValuePerWeightUnits EstimatedUnit { get; set; }

        public ValuePerWeightUnits CostUnit { get; set; }

        public ValuePerWeightUnits? DisposalUnit { get; set; }

        public decimal EstimatedAmount { get; set; }

        public decimal CostAmount { get; set; }

        public decimal? DisposalAmount { get; set; }

        public Guid NotificationId { get; set; }

        public bool IsDisposal { get; set; }
        
        public AddRecoveryInfoToNotification(Guid notificationId, bool isDisposal, 
            ValuePerWeightUnits estimatedUnit, decimal estimatedAmount,
            ValuePerWeightUnits costUnit, decimal costAmount,
            ValuePerWeightUnits? disposalUnit, decimal? disposalAmount)
        {
            NotificationId = notificationId;
            IsDisposal = isDisposal;
            EstimatedUnit = estimatedUnit;
            EstimatedAmount = estimatedAmount;
            CostUnit = costUnit;
            CostAmount = costAmount;
            DisposalUnit = disposalUnit;
            DisposalAmount = disposalAmount;
        }
    }
}