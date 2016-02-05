namespace EA.Iws.Core.Admin
{
    using System;
    using Notification;

    public class InternalUserData
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public InternalUserStatus Status { get; set; }

        public string PhoneNumber { get; set; }

        public string JobTitle { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, Surname); }
        }
    }
}
