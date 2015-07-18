namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.Notification;

    internal class FacilityViewModel
    {
        private AddressViewModel address;

        public FacilityViewModel(Facility facility, int countOfFacilities)
        {
            Name = facility.Business.Name;
            address = new AddressViewModel(facility.Address);
            ContactPerson = facility.Contact.FirstName + " " + facility.Contact.LastName;
            Telephone = facility.Contact.Telephone;
            Fax = facility.Contact.Fax ?? string.Empty;
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
            get { return address.Address(AddressLines.Two); }
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
            ActualSite = (countOfFacilities == 1) ? "As Above" : "See annex.";
        }

        public static FacilityViewModel GetSeeAnnexInstructionForFacility(int annexNumber)
        {
            var seeAnnexNotice = "See annex " + annexNumber;
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
                ActualSite = string.Empty
            };
        }

        public static FacilityViewModel GetSeeAnnexInstructionForFacilityCaseTwoFacilities(FacilityViewModel facility, int annexNumber)
        {
            var seeAnnexNotice = "See annex " + annexNumber;
            facility.ActualSite = seeAnnexNotice;
            return facility;
        }
    }
}