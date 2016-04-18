namespace EA.Iws.Core.Facilities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Shared;

    public class FacilityData
    {
        public Guid Id { get; set; }

        public BusinessInfoData Business { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public Guid NotificationId { get; set; }

        [Display(Name = "Actual site of disposal/recovery")]
        public bool IsActualSiteOfTreatment { get; set; }

        public FacilityData()
        {
                Address = new AddressData();
                Contact = new ContactData();
                Business = new BusinessInfoData();
        }
    }
}
