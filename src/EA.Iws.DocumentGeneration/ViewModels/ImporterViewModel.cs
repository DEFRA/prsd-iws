namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.NotificationApplication;

    internal class ImporterViewModel
    {
        private readonly AddressViewModel address;

        public ImporterViewModel(Importer importer)
        {
            Name = importer.Business.Name;
            address = new AddressViewModel(importer.Address);
            ContactPerson = importer.Contact.FirstName + " " + importer.Contact.LastName;
            Telephone = importer.Contact.Telephone.ToFormattedContact();
            Fax = importer.Contact.Fax.ToFormattedContact();
            Email = importer.Contact.Email;
            RegistrationNumber = importer.Business.RegistrationNumber;
        }

        public string Name { get; private set; }

        public string Address
        {
            get { return address.Address(AddressLines.Four); }
        }

        public string RegistrationNumber { get; private set; }

        public string ContactPerson { get; private set; }

        public string Telephone { get; private set; }

        public string Fax { get; private set; }

        public string Email { get; private set; }
    }
}
