namespace EA.Iws.Core.RecoveryInfo
{
    using System;
    using Shared;

    public class RecoveryInfoData
    {
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public bool IsDisposal { get; set; }

        public ValuePerWeightUnits? EstimatedUnit { get; set; }

        public ValuePerWeightUnits? CostUnit { get; set; }

        public ValuePerWeightUnits? DisposalUnit { get; set; }

        public decimal? EstimatedAmount { get; set; }

        public decimal? CostAmount { get; set; }

        public decimal? DisposalAmount { get; set; }
    }
}
