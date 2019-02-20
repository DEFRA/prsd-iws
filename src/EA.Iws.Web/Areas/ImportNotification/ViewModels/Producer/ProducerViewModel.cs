namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Producer
{
    using System.ComponentModel.DataAnnotations;
    using Core.ImportNotification.Draft;
    using Shared;

    public class ProducerViewModel
    {
        public AddressViewModel Address { get; set; }

        [Display(Name = "BusinessName", ResourceType = typeof(ProducerViewModelResources))]
        public string BusinessName { get; set; }

        public ContactViewModel Contact { get; set; }

        public bool AreMultiple { get; set; }

        public bool IsAddedToAddressBook { get; set; }

        public ProducerViewModel()
        {
        }

        public ProducerViewModel(Producer producer)
        {
            Address = new AddressViewModel(producer.Address);
            AreMultiple = producer.AreMultiple;
            BusinessName = producer.BusinessName;
            Contact = new ContactViewModel(producer.Contact);
            IsAddedToAddressBook = producer.IsAddedToAddressBook;
        }
    }
}