namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using Domain.NotificationApplication;

    public class ProducerFactory
    {
        public static Producer Create(Guid id, ProducerBusiness business = null, Address address = null, Contact contact = null)
        {
            var producer = ObjectInstantiator<Producer>.CreateNew();

            EntityHelper.SetEntityId(producer, id);

            if (business == null)
            {
                business = ObjectFactory.CreateEmptyProducerBusiness();
            }

            if (address == null)
            {
                address = ObjectFactory.CreateDefaultAddress();
            }

            if (contact == null)
            {
                contact = ObjectFactory.CreateEmptyContact();
            }

            ObjectInstantiator<Producer>.SetProperty(x => x.Business, business, producer);
            ObjectInstantiator<Producer>.SetProperty(x => x.Address, address, producer);
            ObjectInstantiator<Producer>.SetProperty(x => x.Contact, contact, producer);

            return producer;
        }
    }
}
