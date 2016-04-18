namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Exporter
{
    using System;
    using Core.Exporters;
    using Core.Shared;
    using Requests.Exporters;
    using Web.ViewModels.Shared;

    public class ExporterViewModel
    {
        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public BusinessTypeViewModel Business { get; set; }

        public bool IsAddedToAddressBook { get; set; }

        public ExporterViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessTypeViewModel();
            Business.DisplayAdditionalNumber = true;
            Business.DisplayCompaniesHouseHint = true;
        }

        public ExporterViewModel(ExporterData exporter)
        {
            NotificationId = exporter.NotificationId;
            Address = exporter.Address;
            Contact = exporter.Contact;
            Business = new BusinessTypeViewModel(exporter.Business);
            Business.DisplayAdditionalNumber = true;
            Business.DisplayCompaniesHouseHint = true;
        }

        public SetExporterForNotification ToRequest()
        {
            return new SetExporterForNotification
            {
                NotificationId = NotificationId,
                Address = Address,
                Business = Business.ToBusinessInfoData(),
                Contact = Contact
            };
        }
    }
}