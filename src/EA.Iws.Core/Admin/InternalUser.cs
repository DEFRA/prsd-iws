namespace EA.Iws.Core.Admin
{
    public class InternalUser
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public bool IsApproved { get; set; }

        public string PhoneNumber { get; set; }

        public string JobTitle { get; set; }

        public string CompetentAuthority { get; set; }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, Surname); }
        }
    }
}
