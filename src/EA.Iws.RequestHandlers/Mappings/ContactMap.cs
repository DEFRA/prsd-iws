namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class ContactMap : IMap<Contact, ContactData>
    {
        public ContactData Map(Contact source)
        {
            var contactData = new ContactData
            {
                FullName = source.FullName,
                Fax = source.Fax,
                Email = source.Email
            };

            if (source.Telephone != null 
                && source.Telephone.Contains("-") 
                && (source.Telephone.Split('-').Length > 0))
            {
                contactData.TelephonePrefix = source.Telephone.Split('-')[0];
                contactData.Telephone = source.Telephone.Split('-')[1];
            }
            else
            {
                contactData.Telephone = source.Telephone;
            }

            if (!string.IsNullOrEmpty(source.Fax) && source.Fax.Contains("-") && (source.Fax.Split('-').Length > 0))
            {
                contactData.FaxPrefix = source.Fax.Split('-')[0];
                contactData.Fax = source.Fax.Split('-')[1];
            }
            else
            {
                contactData.Fax = source.Fax;
            }

            return contactData;
        }
    }
}
