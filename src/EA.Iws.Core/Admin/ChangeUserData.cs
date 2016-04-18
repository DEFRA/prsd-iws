namespace EA.Iws.Core.Admin
{
    public class ChangeUserData
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string OrganisationName { get; set; }

        public string PhoneNumber { get; set; }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }
    }
}
