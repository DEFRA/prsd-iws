namespace EA.Iws.TestHelpers.DomainFakes
{
    using Domain;
    using Domain.NotificationApplication;
    using Helpers;

    public class TestableContact : Contact
    {
        public new string FullName
        {
            get { return base.FullName; }
            set { ObjectInstantiator<Contact>.SetProperty(x => x.FullName, value, this); }
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
                    FullName = "Billy Knuckles",
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
                    FullName = "Mike Merry",
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
                    FullName = "Sinclair Simms",
                    Email = "sinclairsimms@mailinator.com",
                    Telephone = "+35456513"
                };
            }
        }
    }
}
