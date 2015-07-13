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

            SetActualSiteOfTreatment(countOfFacilities);
        }

        private FacilityViewModel()
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

        public string ActualSite { get; private set; }

        public bool IsActualSite { get; private set; }

        private void SetActualSiteOfTreatment(int countOfFacilities)
        {
            ActualSite = (countOfFacilities == 1) ? "As Above" : "See annex.";
        }

        public static FacilityViewModel GetSeeAnnexInstructionForFacility(int annexNumber)
        {
            var seeAnnexNotice = "See annex " + annexNumber;
            return new FacilityViewModel
            {
                ContactPerson = seeAnnexNotice,
                Name = seeAnnexNotice,
                Email = seeAnnexNotice,
                Telephone = seeAnnexNotice,
                RegistrationNumber = seeAnnexNotice,
                address = AddressViewModel.GetAddressViewModelShowingSeeAnnexInstruction(seeAnnexNotice),
                Fax = seeAnnexNotice,
                ActualSite = seeAnnexNotice
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