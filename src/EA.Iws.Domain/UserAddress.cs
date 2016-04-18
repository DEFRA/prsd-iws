namespace EA.Iws.Domain
{
    using NotificationApplication;
    using Prsd.Core.Domain;

    public class UserAddress : Entity
    {
        public UserAddress(Address address)
        {
            Address = address;
        }

        protected UserAddress()
        {
        }

        public virtual Address Address { get; private set; }

        public void UpdateAddress(UserAddress address)
        {
            Address = address.Address;
        }
    }
}