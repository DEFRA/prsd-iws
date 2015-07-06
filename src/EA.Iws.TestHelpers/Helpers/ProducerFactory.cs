namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using Domain;
    using Domain.Notification;

    public class ProducerFactory
    {
        public static Producer Create(Guid id, Business business = null, Address address = null, string name = "AnyName")
        {
            var producer = ObjectInstantiator<Producer>.CreateNew();

            EntityHelper.SetEntityId(producer, id);

            if (business == null)
            {
                business = ComplexTypeFactory.Create<Business>();
                ObjectInstantiator<Business>.SetProperty(x => x.Name, name, business);
            }

            if (address == null)
            {
                address = ComplexTypeFactory.Create<Address>();
            }

            ObjectInstantiator<Producer>.SetProperty(x => x.Business, business, producer);
            ObjectInstantiator<Producer>.SetProperty(x => x.Address, address, producer);

            return producer;
        }
    }
}
