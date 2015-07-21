namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.Notification;

    internal class ProducerViewModel
    {
        private AddressViewModel address;

        public ProducerViewModel(Producer producer, int countOfProducers)
        {
            Name = producer.Business.Name;
            address = new AddressViewModel(producer.Address);
            ContactPerson = producer.Contact.FirstName + " " + producer.Contact.LastName;
            Telephone = producer.Contact.Telephone;
            Fax = producer.Contact.Fax ?? string.Empty;
            Email = producer.Contact.Email;
            RegistrationNumber = producer.Business.RegistrationNumber;
            IsSiteOfGeneration = producer.IsSiteOfExport;
            AnnexMessage = string.Empty;
            SetSiteOfGeneration(countOfProducers);
        }

        private ProducerViewModel()
        {
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

        public string SiteOfGeneration { get; private set; }

        public bool IsSiteOfGeneration { get; private set; }

        public string AnnexMessage { get; private set; }

        private void SetSiteOfGeneration(int countOfProducers)
        {
            string siteOfGenerationInformation;
            if (countOfProducers == 1)
            {
                siteOfGenerationInformation = "Site as above.";
            }
            else
            {
                siteOfGenerationInformation = "See Annex";
            }

            SiteOfGeneration = siteOfGenerationInformation;
        }

        public static ProducerViewModel GetProducerViewModelShowingSeeAnnexInstruction(int annexNumber)
        {
            var seeAnnexNotice = "See Annex " + annexNumber;

            return new ProducerViewModel
            {
                AnnexMessage = seeAnnexNotice,
                ContactPerson = string.Empty,
                Name = string.Empty,
                Email = string.Empty,
                Telephone = string.Empty,
                RegistrationNumber = string.Empty,
                address = AddressViewModel.GetAddressViewModelShowingSeeAnnexInstruction(string.Empty),
                Fax = string.Empty,
                SiteOfGeneration = string.Empty
            };
        }
    }
}