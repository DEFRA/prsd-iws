namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Domain;
    using Domain.Movement;
    using Helpers;

    public class TestableMovementDetails : MovementDetails
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<MovementDetails>.SetProperty(x => x.Id, value, this); }
        }

        public new ShipmentQuantity ActualQuantity
        {
            get { return base.ActualQuantity; }
            set { ObjectInstantiator<MovementDetails>.SetProperty(x => x.ActualQuantity, value, this); }
        }
    }
}