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

        private void SetSiteOfGeneration(int countOfProducers)
        {
            string siteOfGenerationInformation;
            if (countOfProducers == 1)
            {
                siteOfGenerationInformation = "Site as above.";
            }
            else
            {
                siteOfGenerationInformation = "See annex.";
            }

            SiteOfGeneration = siteOfGenerationInformation;
        }

        public static ProducerViewModel GetProducerViewModelShowingSeeAnnexInstruction(int annexNumber)
        {
            var seeAnnexNotice = "See annex " + annexNumber;

            return new ProducerViewModel
            {
                ContactPerson = seeAnnexNotice,
                Name = seeAnnexNotice,
                Email = seeAnnexNotice,
                Telephone = seeAnnexNotice,
                RegistrationNumber = seeAnnexNotice,
                address = AddressViewModel.GetAddressViewModelShowingSeeAnnexInstruction(seeAnnexNotice),
                Fax = seeAnnexNotice,
                SiteOfGeneration = seeAnnexNotice
            };
        }
    }
}