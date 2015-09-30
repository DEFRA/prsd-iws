namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication.WasteRecovery;
    using Helpers;

    public class TestableWasteRecovery : WasteRecovery
    {
        public new Guid NotificationId
        {
            get { return base.NotificationId; }
            set { ObjectInstantiator<WasteRecovery>.SetProperty(x => x.NotificationId, value, this); }
        }

        public new EstimatedValue EstimatedValue
        {
            get { return base.EstimatedValue; }
            set { ObjectInstantiator<WasteRecovery>.SetProperty(x => x.EstimatedValue, value, this); }
        }

        public new RecoveryCost RecoveryCost
        {
            get { return base.RecoveryCost; }
            set { ObjectInstantiator<WasteRecovery>.SetProperty(x => x.RecoveryCost, value, this); }
        }

        public new DisposalCost DisposalCost
        {
            get { return base.DisposalCost; }
            set { ObjectInstantiator<WasteRecovery>.SetProperty(x => x.DisposalCost, value, this); }
        }
    }
}
