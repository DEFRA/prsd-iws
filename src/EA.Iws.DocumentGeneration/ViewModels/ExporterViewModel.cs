﻿namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.NotificationApplication.Exporter;

    internal class ExporterViewModel
    {
        private readonly AddressViewModel address;

        public ExporterViewModel(Exporter exporter)
        {
            Name = exporter.Business.Name;
            address = new AddressViewModel(exporter.Address);
            ContactPerson = exporter.Contact.FullName;
            Telephone = exporter.Contact.Telephone.ToFormattedContact();
            Fax = exporter.Contact.Fax.ToFormattedContact();
            Email = exporter.Contact.Email;
            RegistrationNumber = exporter.Business.RegistrationNumber;
        }

        public string Name { get; private set; }

        public string Address
        {
            get { return address.Address(AddressLines.Multiple); }
        }

        public string RegistrationNumber { get; private set; }

        public string ContactPerson { get; private set; }

        public string Telephone { get; private set; }

        public string Fax { get; private set; }

        public string Email { get; private set; }
    }
}