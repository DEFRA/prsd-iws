namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Exporter
{
    using System;
    using Requests.Exporters;
    using Requests.Shared;
    using Web.ViewModels.Shared;

    public class ExporterViewModel
    {
        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public BusinessTypeViewModel Business { get; set; }

        public ExporterViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessTypeViewModel();
        }

        public ExporterViewModel(ExporterData exporter)
        {
            NotificationId = exporter.NotificationId;
            Address = exporter.Address;
            Contact = exporter.Contact;
            Business = new BusinessTypeViewModel(exporter.Business);
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