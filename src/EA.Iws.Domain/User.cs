namespace EA.Iws.Domain
{
    using System;
    using Prsd.Core;

    public class User
    {
        protected User()
        {
        }

        public string Id { get; private set; }

        public string FirstName { get; private set; }

        public string Surname { get; private set; }

        public string PhoneNumber { get; private set; }

        public string Email { get; private set; }

        public bool PhoneNumberConfirmed { get; private set; }

        public bool EmailConfirmed { get; private set; }

        public bool TwoFactorEnabled { get; private set; }

        public bool LockoutEnabled { get; private set; }

        public int AccessFailedCount { get; private set; }

        public string UserName { get; private set; }

        public virtual Organisation Organisation { get; private set; }

        public void LinkToOrganisation(Organisation organisation)
        {
            Guard.ArgumentNotNull(() => organisation, organisation);

            if (Organisation != null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "User {0} is already linked to an organisation and may not be linked to another. This user is linked to organisation: {1}",
                        Id, Organisation.Id));
            }

            Organisation = organisation;
        }

        public void UpdateOrganisationOfUser(Organisation organisation)
        {
            Guard.ArgumentNotNull(() => organisation, organisation);
            Organisation = organisation;
        }

        public void LinkToAddress(UserAddress address)
        {
            Guard.ArgumentNotNull(() => address, address);

            Address = address;
        }

        public virtual UserAddress Address { get; private set; }
    }
}