namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using Domain;
    using Domain.NotificationApplication;

    public class ExporterFactory
    {
        public static Exporter Create(Guid id, Business business = null, Address address = null, string name = "AnyName")
        {
            var exporter = ObjectInstantiator<Exporter>.CreateNew();

            EntityHelper.SetEntityId(exporter, id);

            if (business == null)
            {
                business = ComplexTypeFactory.Create<Business>();
                ObjectInstantiator<Business>.SetProperty(x => x.Name, name, business);
            }

            if (address == null)
            {
                address = ComplexTypeFactory.Create<Address>();
            }

            ObjectInstantiator<Exporter>.SetProperty(x => x.Business, business, exporter);
            ObjectInstantiator<Exporter>.SetProperty(x => x.Address, address, exporter);
            ObjectInstantiator<Exporter>.SetProperty(x => x.Contact, ComplexTypeFactory.Create<Contact>(), exporter);

            return exporter;
        }
    }
}
