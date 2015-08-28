namespace EA.Iws.TestHelpers.DomainFakes
{
    using Core.RecoveryInfo;
    using Domain.NotificationApplication;
    using Helpers;

    public class TestableRecoveryInfo : RecoveryInfo
    {
        public new RecoveryInfoUnits EstimatedUnit
        {
            get { return base.EstimatedUnit; }
            set { ObjectInstantiator<RecoveryInfo>.SetProperty(x => x.EstimatedUnit, value, this); }
        }

        public new RecoveryInfoUnits CostUnit
        {
            get { return base.CostUnit; }
            set { ObjectInstantiator<RecoveryInfo>.SetProperty(x => x.CostUnit, value, this); }
        }

        public new RecoveryInfoUnits? DisposalUnit
        {
            get { return base.DisposalUnit; }
            set { ObjectInstantiator<RecoveryInfo>.SetProperty(x => x.DisposalUnit, value, this); }
        }

        public new decimal EstimatedAmount
        {
            get { return base.EstimatedAmount; }
            set { ObjectInstantiator<RecoveryInfo>.SetProperty(x => x.EstimatedAmount, value, this); }
        }

        public new decimal CostAmount
        {
            get { return base.CostAmount; }
            set { ObjectInstantiator<RecoveryInfo>.SetProperty(x => x.CostAmount, value, this); }
        }

        public new decimal? DisposalAmount
        {
            get { return base.DisposalAmount; }
            set { ObjectInstantiator<RecoveryInfo>.SetProperty(x => x.DisposalAmount, value, this); }
        }
    }
}
