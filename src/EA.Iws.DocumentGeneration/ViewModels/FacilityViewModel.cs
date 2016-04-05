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
    }
}