namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.NotificationApplication;

    internal class FacilityViewModel
    {
        private AddressViewModel address;

        public FacilityViewModel(Facility facility, int countOfFacilities)
        {
            Name = facility.Business.Name;
            address = new AddressViewModel(facility.Address);
            ContactPerson = facility.Contact.FullName;
            Telephone = facility.Contact.Telephone.ToFormattedContact();
            Fax = facility.Contact.Fax.ToFormattedContact();
            Email = facility.Contact.Email;
            RegistrationNumber = facility.Business.RegistrationNumber;
            IsActualSite = facility.IsActualSiteOfTreatment;
            AnnexMessage = string.Empty;
            SetActualSiteOfTreatment(countOfFacilities);
        }

        private FacilityViewModel()
        {
        }

        public string Name { get; private set; }

        public string Address
        {
            get { return address.Address(AddressLines.Single); }
        }

        public string RegistrationNumber { get; private set; }

        public string ContactPerson { get; private set; }

        public string Telephone { get; private set; }

        public string Fax { get; private set; }

        public string Email { get; private set; }

        public string ActualSite { get; private set; }

        public bool IsActualSite { get; private set; }

        public string AnnexMessage { get; private set; }

        private void SetActualSiteOfTreatment(int countOfFacilities)
        {
            ActualSite = (countOfFacilities == 1) ? "Site as above" : "See Annex";
        }

        public static FacilityViewModel GetSeeAnnexInstructionForFacility(int annexNumber)
        {
            var seeAnnexNotice = "See Annex " + annexNumber;
            return new FacilityViewModel
            {
                AnnexMessage = seeAnnexNotice,
                ContactPerson = string.Empty,
                Name = string.Empty,
                Email = string.Empty,
                Telephone = string.Empty,
                RegistrationNumber = string.Empty,
                address = AddressViewModel.GetAddressViewModelShowingSeeAnnexInstruction(string.Empty),
                Fax = string.Empty,
                ActualSite = seeAnnexNotice
            };
        }

        public static FacilityViewModel GetSeeAnnexInstructionForFacilityCaseTwoFacilities(FacilityViewModel facility, int annexNumber)
        {
            var seeAnnexNotice = "See Annex " + annexNumber;
            facility.ActualSite = seeAnnexNotice;
            return facility;
        }

        /// <summary>
        /// Returns a FacilityViewModel that displays the first facility's full details
        /// in Block 10, with a "See Annex" reference for the actual site of treatment.
        /// </summary>
        public static FacilityViewModel GetFirstFacilityWithAnnexReference(FacilityViewModel facility, int annexNumber)
        {
            var seeAnnexNotice = "See Annex " + annexNumber;
            return new FacilityViewModel
            {
                Name = facility.Name,
                address = facility.address,
                RegistrationNumber = facility.RegistrationNumber,
                ContactPerson = facility.ContactPerson,
                Telephone = facility.Telephone,
                Fax = facility.Fax ?? string.Empty,
                Email = facility.Email,
                IsActualSite = facility.IsActualSite,
                ActualSite = seeAnnexNotice,
                AnnexMessage = string.Empty
            };
        }
    }
}