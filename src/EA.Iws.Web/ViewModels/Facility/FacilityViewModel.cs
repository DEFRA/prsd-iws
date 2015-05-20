namespace EA.Iws.Web.ViewModels.Facility
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Requests.Shared;
    using Shared;

    public class FacilityViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Actual site of disposal/recovery")]
        public bool IsActualSiteOfTreatment { get; set; }

        public NotificationType NotificationType { get; set; }

        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public BusinessViewModel Business { get; set; }

        public FacilityViewModel()
        {
            if (Address == null)
            {
                Address = new AddressData();
            }

            if (Contact == null)
            {
                Contact = new ContactData();
            }

            if (Business == null)
            {
                Business = new BusinessViewModel();
            }
        }
    }
}