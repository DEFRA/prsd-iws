namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System;
    using Domain.Notification;

    internal class CarrierViewModel
    {
        private AddressViewModel address;

        public CarrierViewModel()
        {
        }

        public CarrierViewModel(Carrier carrier, string meansOfTransport)
        {
            Name = carrier.Business.Name;
            address = new AddressViewModel(carrier.Address);
            ContactPerson = carrier.Contact.FirstName + " " + carrier.Contact.LastName;
            Telephone = carrier.Contact.Telephone;
            Fax = carrier.Contact.Fax ?? string.Empty;
            Email = carrier.Contact.Email;
            RegistrationNumber = carrier.Business.RegistrationNumber;
            MeansOfTransport = meansOfTransport;
            AnnexMessage = string.Empty;
        }

        public string Name { get; private set; }

        public string Address
        {
            get { return address.Address(AddressLines.Three); }
        }

        public string RegistrationNumber { get; private set; }

        public string ContactPerson { get; private set; }

        public string Telephone { get; private set; }

        public string Fax { get; private set; }

        public string Email { get; private set; }

        public string MeansOfTransport { get; private set; }

        public string AnnexMessage { get; private set; }

        public static CarrierViewModel GetCarrierViewModelShowingSeeAnnexInstruction(int annexNumber, string meansOfTransport)
        {
            var seeAnnexNotice = "See annex " + annexNumber;

            return new CarrierViewModel
            {
                AnnexMessage = seeAnnexNotice,
                ContactPerson = string.Empty,
                Name = string.Empty,
                Email = string.Empty,
                Telephone = string.Empty,
                RegistrationNumber = string.Empty,
                address = AddressViewModel.GetAddressViewModelShowingSeeAnnexInstruction(string.Empty),
                Fax = string.Empty,
                MeansOfTransport = meansOfTransport
            };
        }
    }
}
