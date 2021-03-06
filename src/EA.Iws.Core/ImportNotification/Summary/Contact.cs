﻿namespace EA.Iws.Core.ImportNotification.Summary
{
    public class Contact
    {
        public string Name { get; set; }

        public string Telephone { get; set; }

        public string TelephonePrefix { get; set; }

        public string Email { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(Name)
                   && string.IsNullOrWhiteSpace(Telephone)
                   && string.IsNullOrWhiteSpace(Email);
        }

        public static Contact FromDraftContact(Draft.Contact contact)
        {
            if (contact == null)
            {
                return new Contact();
            }

            return new Contact
            {
                Email = contact.Email,
                Telephone = contact.Telephone,
                TelephonePrefix = contact.TelephonePrefix,
                Name = contact.ContactName
            };
        }
    }
}
