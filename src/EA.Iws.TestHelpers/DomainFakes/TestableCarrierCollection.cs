namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.NotificationApplication;

    public class TestableCarrierCollection : CarrierCollection
    {
        public TestableCarrierCollection(Guid notificationId) : base(notificationId)
        {
        }

        public new IEnumerable<Carrier> Carriers
        {
            get { return base.Carriers; }
            set { CarriersCollection = value.ToList(); }
        }
    }
}