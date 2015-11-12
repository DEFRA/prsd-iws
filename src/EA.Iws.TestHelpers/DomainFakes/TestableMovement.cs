namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Core.Movement;
    using Domain.Movement;
    using Helpers;

    public class TestableMovement : Movement
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.Id, value, this); }
        }

        public new Guid NotificationId
        {
            get { return base.NotificationId; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.NotificationId, value, this); }
        }

        public new DateTime Date
        {
            get { return base.Date; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.Date, value, this); }
        }

        public new int Number
        {
            get { return base.Number; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.Number, value, this); }
        }

        public new MovementReceipt Receipt
        {
            get { return base.Receipt; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.Receipt, value, this); }
        }

        public new MovementCompletedReceipt CompletedReceipt
        {
            get { return base.CompletedReceipt; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.CompletedReceipt, value, this); }
        }

        public new MovementStatus Status
        {
            get { return base.Status; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.Status, value, this); }
        }
    }
}