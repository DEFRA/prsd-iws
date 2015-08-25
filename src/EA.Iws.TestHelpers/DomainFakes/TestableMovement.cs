namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Domain.Movement;
    using Helpers;

    public class TestableMovement : Movement
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.Id, value, this); }
        }

        public new Guid NotificationApplicationId
        {
            get { return base.NotificationApplicationId; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.NotificationApplicationId, value, this); }
        }

        public new decimal Quantity
        {
            get { return base.Quantity; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.Quantity, value, this); }
        }

        public new DateTime Date
        {
            get { return base.Date; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.Date, value, this); }
        }
    }
}
