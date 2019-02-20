namespace EA.Iws.Core.ImportNotification.Draft
{
    public class Contact
    {
        public string ContactName { get; set; }

        public string Telephone { get; set; }

        public string TelephonePrefix { get; set; }

        public string Email { get; set; }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(ContactName)
                    && string.IsNullOrEmpty(Telephone)
                    && string.IsNullOrEmpty(TelephonePrefix)
                    && string.IsNullOrEmpty(Email);
            }
        }
    }
}