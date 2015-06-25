namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Facility
{
    using System;
    using Core.Shared;
    using Requests.Facilities;
    using Requests.Shared;
    using Web.ViewModels.Shared;

    public class AddFacilityViewModel
    {
        public AddFacilityViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessViewModel();
        }

        public NotificationType NotificationType { get; set; }

        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public BusinessViewModel Business { get; set; }

        public AddFacilityToNotification ToRequest()
        {
            return new AddFacilityToNotification
            {
                Address = Address,
                Business = (BusinessData)Business,
                Contact = Contact,
                NotificationId = NotificationId
            };
        }
    }
}