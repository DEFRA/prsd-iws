namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.Producer
{
    using System;
    using Core.Shared;
    using EA.Iws.Core.Notification;
    using NotificationApplication.ViewModels.Producer;

    public class AddProducerViewModel
    {
        public AddProducerViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new ProducerBusinessTypeViewModel();
        }

        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public ProducerBusinessTypeViewModel Business { get; set; }

        public bool IsAddedToAddressBook { get; set; }

        public AdditionalChargeData AdditionalCharge { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public bool ShowAdditionalCharge { get; set; }
    }
}