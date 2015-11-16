namespace EA.Iws.Core.ImportNotification.Summary
{
    public class Contact
    {
        public string Name { get; set; }

        public string Telephone { get; set; }

        public string Email { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(Name)
                   && string.IsNullOrWhiteSpace(Telephone)
                   && string.IsNullOrWhiteSpace(Email);
        }

        public static Contact FromDraftContact(Draft.Contact contact)
        {
            return new Contact
            {
                Email = contact.Email,
                Telephone = contact.Telephone,
                Name = contact.ContactName
            };
        }
    }
}
