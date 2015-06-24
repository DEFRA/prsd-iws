namespace EA.Iws.Requests.RecoveryInfo
{
    using System;
    using Core.RecoveryInfo;

    public class RecoveryInfoData
    {
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public bool IsDisposal { get; set; }

        public RecoveryInfoUnits EstimatedUnit { get; set; }

        public RecoveryInfoUnits CostUnit { get; set; }

        public RecoveryInfoUnits? DisposalUnit { get; set; }

        public decimal EstimatedAmount { get; set; }

        public decimal CostAmount { get; set; }

        public decimal? DisposalAmount { get; set; }
    }
}
