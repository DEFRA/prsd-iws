namespace EA.Iws.Web.ViewModels.Facility
{
    using System;
    using Requests.Facilities;
    using Requests.Shared;
    using Shared;

    public class EditFacilityViewModel : AddFacilityViewModel
    {
        public EditFacilityViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessViewModel();
        }

        public EditFacilityViewModel(FacilityData facility)
        {
            Address = facility.Address;
            Business = (BusinessViewModel)facility.Business;
            Contact = facility.Contact;
            FacilityId = facility.Id;
            NotificationId = facility.NotificationId;
        }

        public Guid FacilityId { get; set; }

        public new UpdateFacilityForNotification ToRequest()
        {
            return new UpdateFacilityForNotification
            {
                Address = Address,
                Business = (BusinessData)Business,
                Contact = Contact,
                FacilityId = FacilityId,
                NotificationId = NotificationId
            };
        }
    }
}