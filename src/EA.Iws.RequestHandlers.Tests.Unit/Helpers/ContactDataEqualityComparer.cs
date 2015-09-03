namespace EA.Iws.RequestHandlers.Tests.Unit.Helpers
{
    using System.Collections.Generic;
    using Core.Shared;

    public class ContactDataEqualityComparer : IEqualityComparer<ContactData>
    {
        public bool Equals(ContactData x, ContactData y)
        {                
            if (x == y)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Email == y.Email
                   && x.Fax == y.Fax
                   && x.FaxPrefix == y.FaxPrefix
                   && x.FirstName == y.FirstName
                   && x.LastName == y.LastName
                   && x.Telephone == y.Telephone
                   && x.TelephonePrefix == y.TelephonePrefix;
        }

        public int GetHashCode(ContactData obj)
        {
            return base.GetHashCode();
        }
    }
}