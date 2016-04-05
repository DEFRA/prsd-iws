namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.NotificationApplication;

    internal class ProducerViewModel
    {
        private AddressViewModel address;
        private const int TextMaxLength = 100;

        public ProducerViewModel(Producer producer, int countOfProducers, string processText, bool? isIsProcessAnnexAttachedAttached)
        {
            Name = producer.Business.Name;
            address = new AddressViewModel(producer.Address);
            ContactPerson = producer.Contact.FullName;
            Telephone = producer.Contact.Telephone.ToFormattedContact();
            Fax = producer.Contact.Fax.ToFormattedContact();
            Email = producer.Contact.Email;
            RegistrationNumber = producer.Business.RegistrationNumber;
            IsSiteOfGeneration = producer.IsSiteOfExport;
            AnnexMessage = string.Empty;
            SetSiteOfGeneration(countOfProducers);
            CountOfProducers = countOfProducers;
            ProcessOfGeneration = processText ?? string.Empty;
            IsProcessAnnexAttached = isIsProcessAnnexAttachedAttached;
            DescriptionTitle = "Process of generation";
        }

        public string Name { get; private set; }

        public string Address
        {
            get { return address.Address(AddressLines.Three); }
        }

        public string DescriptionTitle { get; set; }

        public string RegistrationNumber { get; private set; }

        public string ContactPerson { get; private set; }

        public string Telephone { get; private set; }

        public string Fax { get; private set; }

        public string Email { get; private set; }

        public string SiteOfGeneration { get; private set; }

        public bool IsSiteOfGeneration { get; private set; }

        public string AnnexMessage { get; private set; }

        public int CountOfProducers { get; private set; }

        public string ProcessOfGeneration { get; private set; }

        public bool? IsProcessAnnexAttached { get; private set; }

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

        private string GetSiteAndProcessText(int annexNumber)
        {
            string text = string.Empty;
            bool isProcessAnnexAttached = false;

            if (IsProcessAnnexAttached.HasValue)
            {
                isProcessAnnexAttached = IsProcessAnnexAttached.Value;
            }

            if (CountOfProducers == 1)
            {
                if (ProcessOfGeneration.Length > TextMaxLength)
                {
                    text = "Site as above. See Annex " + annexNumber;
                }

                if (isProcessAnnexAttached && ProcessOfGeneration.Length <= TextMaxLength)
                {
                    text = "Site as above.  See Annex " + annexNumber + ".  " + ProcessOfGeneration;
                }

                if (!isProcessAnnexAttached && ProcessOfGeneration.Length <= TextMaxLength)
                {
                    text = "Site as above.  " + ProcessOfGeneration;
                }
            }

            if (CountOfProducers > 1)
            {
                if (ProcessOfGeneration.Length > TextMaxLength)
                {
                    text = "See Annex " + annexNumber;
                }

                if (ProcessOfGeneration.Length <= TextMaxLength)
                {
                    text = "See Annex " + annexNumber + ".  " + ProcessOfGeneration;
                }
            }

            return text;
        }

        public ProducerViewModel GetProducerViewModelShowingAnnexMessages(int producerCount, ProducerViewModel pvm, int annexNumber)
        {
            //If there is only one let put it on the form otherwise put them all in the annex - one has been removed hence > 1 here
            if (producerCount > 1)
            {
                return GetProducerViewModelShowingAnnexMessagesForProducerCountGreaterThanTwo(pvm, annexNumber);
            }

            return GetProducerViewModelShowingAnnexMessagesForProducerCountNotGreaterThanTwo(pvm, annexNumber);
        }

        private ProducerViewModel GetProducerViewModelShowingAnnexMessagesForProducerCountGreaterThanTwo(ProducerViewModel pvm, int annexNumber)
        {
            var seeAnnexNotice = "See Annex " + annexNumber;

            pvm.AnnexMessage = seeAnnexNotice;
            pvm.ContactPerson = string.Empty;
            pvm.Name = string.Empty;
            pvm.Email = string.Empty;
            pvm.Telephone = string.Empty;
            pvm.RegistrationNumber = string.Empty;
            pvm.address = AddressViewModel.GetAddressViewModelShowingSeeAnnexInstruction(string.Empty);
            pvm.Fax = string.Empty;
            pvm.SiteOfGeneration = GetSiteAndProcessText(annexNumber);

            return pvm;
        }

        private ProducerViewModel GetProducerViewModelShowingAnnexMessagesForProducerCountNotGreaterThanTwo(ProducerViewModel pvm, int annexNumber)
        {
            pvm.SiteOfGeneration = GetSiteAndProcessText(annexNumber);
            return pvm;
        }

        public static int ProcessOfGenerationMaxTextLength()
        {
            return TextMaxLength;
        }
    }
}