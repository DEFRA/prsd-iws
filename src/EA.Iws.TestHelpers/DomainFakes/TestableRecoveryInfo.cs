namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication.Recovery;
    using Helpers;

    public class TestableRecoveryInfo : RecoveryInfo
    {
        public new Guid NotificationId
        {
            get { return base.NotificationId; }
            set { ObjectInstantiator<RecoveryInfo>.SetProperty(x => x.NotificationId, value, this); }
        }

        public new EstimatedValue EstimatedValue
        {
            get { return base.EstimatedValue; }
            set { ObjectInstantiator<RecoveryInfo>.SetProperty(x => x.EstimatedValue, value, this); }
        }

        public new RecoveryCost RecoveryCost
        {
            get { return base.RecoveryCost; }
            set { ObjectInstantiator<RecoveryInfo>.SetProperty(x => x.RecoveryCost, value, this); }
        }

        public new DisposalCost DisposalCost
        {
            get { return base.DisposalCost; }
            set { ObjectInstantiator<RecoveryInfo>.SetProperty(x => x.DisposalCost, value, this); }
        }
    }
}
