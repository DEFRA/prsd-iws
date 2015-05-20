namespace EA.Iws.RequestHandlers.Mappings
{
    using Domain;
    using Requests.Shared;

    internal class ValueObjectInitializer
    {
        public static Business CreateBusiness(BusinessData business)
        {
            return new Business(business.Name, business.EntityType, business.RegistrationNumber, business.AdditionalRegistrationNumber);
        }

        public static Contact CreateContact(ContactData contact)
        {
            return new Contact(contact.FirstName, contact.LastName, contact.Telephone, contact.Email, contact.Fax);
        }

        public static Address CreateAddress(AddressData address, string countryName)
        {
            return new Address(address.Building,
                address.StreetOrSuburb,
                address.Address2,
                address.TownOrCity,
                address.PostalCode,
                address.CountryName ?? countryName);
        }
    }
}
