namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Domain.NotificationApplication;
    using Formatters;

    internal class CarrierViewModel
    {
        private AddressViewModel address;
        private string name = string.Empty;
        private string registrationNumber = string.Empty;
        private string contactPerson = string.Empty;
        private string telephone = string.Empty;
        private string fax = string.Empty;
        private string email = string.Empty;
        private string meansOfTransport = string.Empty;
        private string annexMessage = string.Empty;

        public CarrierViewModel()
        {
        }

        public static IList<CarrierViewModel> CreateCarrierViewModelsForNotification(
            NotificationApplication notification,
            MeansOfTransportFormatter meansOfTransportFormatter)
        {
            if (notification == null
                || notification.Carriers == null
                || !notification.Carriers.Any())
            {
                return new CarrierViewModel[0];
            }

            var meansOfTransport = meansOfTransportFormatter.MeansOfTransportAsString(notification.MeansOfTransport);

            return notification.Carriers.Select(c => new CarrierViewModel(c, meansOfTransport))
                .ToArray();
        }

        public CarrierViewModel(Carrier carrier, string meansOfTransport)
        {
            if (carrier == null)
            {
                address = new AddressViewModel(null);
                return;
            }

            if (!string.IsNullOrWhiteSpace(meansOfTransport))
            {
                MeansOfTransport = meansOfTransport;
            }

            SetBusinessFields(carrier.Business);
            SetContactFields(carrier.Contact);
            address = new AddressViewModel(carrier.Address);
        }

        public void SetBusinessFields(Business business)
        {
            if (business == null)
            {
                return;
            }

            Name = business.Name ?? string.Empty;
            RegistrationNumber = business.RegistrationNumber ?? string.Empty;
        }

        public void SetContactFields(Contact contact)
        {
            if (contact == null)
            {
                return;
            }

            ContactPerson = contact.FirstName + " " + contact.LastName;
            Telephone = contact.Telephone ?? string.Empty;
            Fax = contact.Fax ?? string.Empty;
            Email = contact.Email ?? string.Empty;
        }

        public string Name
        {
            get { return name; }
            private set { name = value; }
        }

        public string Address
        {
            get { return address.Address(AddressLines.Three); }
        }

        public string RegistrationNumber
        {
            get { return registrationNumber; }
            private set { registrationNumber = value; }
        }

        public string ContactPerson
        {
            get { return contactPerson; }
            private set { contactPerson = value; }
        }

        public string Telephone
        {
            get { return telephone.ToFormattedContact(); }
            private set { telephone = value; }
        }

        public string Fax
        {
            get { return fax.ToFormattedContact(); }
            private set { fax = value; }
        }

        public string Email
        {
            get { return email; }
            private set { email = value; }
        }

        public string MeansOfTransport
        {
            get { return meansOfTransport; }
            private set { meansOfTransport = value; }
        }

        public string AnnexMessage
        {
            get { return annexMessage; }
            private set { annexMessage = value; }
        }

        public static CarrierViewModel GetCarrierViewModelShowingSeeAnnexInstruction(int annexNumber, string meansOfTransport)
        {
            var seeAnnexNotice = "See Annex " + annexNumber;

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
