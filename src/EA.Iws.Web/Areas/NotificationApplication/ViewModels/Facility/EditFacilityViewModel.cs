namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Facility
{
    using System;
    using Core.Facilities;
    using Core.Shared;
    using Requests.Facilities;
    using Web.ViewModels.Shared;

    public class EditFacilityViewModel : AddFacilityViewModel
    {
        public EditFacilityViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessTypeViewModel();
        }

        public EditFacilityViewModel(FacilityData facility)
        {
            Address = facility.Address;
            Business = new BusinessTypeViewModel(facility.Business);
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
                Business = Business.ToBusinessInfoData(),
                Contact = Contact,
                FacilityId = FacilityId,
                NotificationId = NotificationId
            };
        }
    }
}