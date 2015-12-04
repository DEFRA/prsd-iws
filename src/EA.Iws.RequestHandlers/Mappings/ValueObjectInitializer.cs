namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using BusinessType = Core.Shared.BusinessType;

    internal class ValueObjectInitializer
    {
        public static Business CreateBusiness(BusinessInfoData business)
        {
            if (business.BusinessType == BusinessType.Other)
            {
                return Business.CreateOtherBusiness(business.Name, business.RegistrationNumber,
                    business.AdditionalRegistrationNumber, business.OtherDescription);
            }

            return Business.CreateBusiness(business.Name, Domain.NotificationApplication.BusinessType.FromBusinessType(business.BusinessType), 
                business.RegistrationNumber, business.AdditionalRegistrationNumber);
        }

        public static Contact CreateContact(ContactData contact)
        {
            contact.Telephone = contact.TelephonePrefix + "-" + contact.Telephone;
            if (!string.IsNullOrWhiteSpace(contact.Fax))
            {
                contact.Fax = contact.FaxPrefix + "-" + contact.Fax;
            }
            return new Contact(contact.FirstName, contact.LastName, contact.Telephone, contact.Email, contact.Fax);
        }

        public static Address CreateAddress(AddressData address, string countryName)
        {
            return new Address(address.StreetOrSuburb,
                address.Address2,
                address.TownOrCity,
                address.Region,
                address.PostalCode,
                address.CountryName ?? countryName);
        }
    }
}
