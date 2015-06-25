namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using Core.Shared;
    using Domain;
    using Requests.Shared;
    using BusinessType = Core.Shared.BusinessType;

    internal class ValueObjectInitializer
    {
        public static Business CreateBusiness(BusinessData business)
        {
            return new Business(business.Name, business.EntityType, business.RegistrationNumber, business.AdditionalRegistrationNumber);
        }

        public static Business CreateBusiness(BusinessInfoData business)
        {
            if (business.BusinessType == BusinessType.Other)
            {
                return Business.CreateOtherBusiness(business.Name, business.RegistrationNumber,
                    business.AdditionalRegistrationNumber, business.OtherDescription);
            }

            return Business.CreateBusiness(business.Name, GetBusinessType(business.BusinessType),
                business.RegistrationNumber, business.AdditionalRegistrationNumber);
        }

        private static Domain.BusinessType GetBusinessType(BusinessType businessType)
        {
            switch (businessType)
            {
                case BusinessType.LimitedCompany:
                    return Domain.BusinessType.LimitedCompany;
                case BusinessType.SoleTrader:
                    return Domain.BusinessType.SoleTrader;
                case BusinessType.Partnership:
                    return Domain.BusinessType.Partnership;
                case BusinessType.Other:
                    return Domain.BusinessType.Other;
                default:
                    throw new ArgumentException(string.Format("Unknown business type: {0}", businessType), "businessType");
            }
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
                address.Region,
                address.PostalCode,
                address.CountryName ?? countryName);
        }
    }
}
