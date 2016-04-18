namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Importer;

    public class ImporterFactory
    {
        public static Importer Create(Guid notiificationId, Guid id, Business business = null, Address address = null, string name = "AnyName")
        {
            if (business == null)
            {
                business = ComplexTypeFactory.Create<Business>();
                ObjectInstantiator<Business>.SetProperty(x => x.Name, name, business);
            }

            if (address == null)
            {
                address = ComplexTypeFactory.Create<Address>();
            }

            var importer = new Importer(notiificationId, address, business, ComplexTypeFactory.Create<Contact>());

            EntityHelper.SetEntityId(importer, id);

            return importer;
        }
    }
}
