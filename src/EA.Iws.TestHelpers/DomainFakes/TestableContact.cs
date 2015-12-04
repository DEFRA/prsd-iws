namespace EA.Iws.TestHelpers.DomainFakes
{
    using Domain;
    using Domain.NotificationApplication;
    using Helpers;

    public class TestableContact : Contact
    {
        public new string FirstName
        {
            get { return base.FirstName; }
            set { ObjectInstantiator<Contact>.SetProperty(x => x.FirstName, value, this); }
        }

        public new string LastName
        {
            get { return base.LastName; }
            set { ObjectInstantiator<Contact>.SetProperty(x => x.LastName, value, this); }
        }

        public new string Telephone
        {
            get { return base.Telephone; }
            set { ObjectInstantiator<Contact>.SetProperty(x => x.Telephone, value, this); }
        }

        public new string Fax
        {
            get { return base.Fax; }
            set { ObjectInstantiator<Contact>.SetProperty(x => x.Fax, value, this); }
        }

        public new string Email
        {
            get { return base.Email; }
            set { ObjectInstantiator<Contact>.SetProperty(x => x.Email, value, this); }
        }

        public static Contact BillyKnuckles
        {
            get
            {
                return new TestableContact
                {
                    FirstName = "Billy",
                    LastName = "Knuckles",
                    Email = "billyk@mail.com",
                    Telephone = "+4401858425"
                };
            }
        }

        public static Contact MikeMerry
        {
            get
            {
                return new TestableContact
                {
                    FirstName = "Mike",
                    LastName = "Merry",
                    Email = "mike@murrays.com",
                    Telephone = "+4453119321",
                    Fax = "+4423465403"
                };
            }
        }

        public static Contact SinclairSimms
        {
            get
            {
                return new TestableContact
                {
                    FirstName = "Sinclair",
                    LastName = "Simms",
                    Email = "sinclairsimms@mailinator.com",
                    Telephone = "+35456513"
                };
            }
        }
    }
}
