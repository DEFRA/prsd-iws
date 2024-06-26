﻿namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Producer
{
    using Core.ImportNotification.Draft;
    using EA.Iws.Core.Shared;
    using Shared;
    using System.ComponentModel.DataAnnotations;

    public class ProducerViewModel
    {
        public AddressViewModel Address { get; set; }

        public BusinessViewModel Business { get; set; }

        public ContactViewModel Contact { get; set; }

        public bool AreMultiple { get; set; }

        public bool IsAddedToAddressBook { get; set; }

        [Display(Name = "Organisation type")]
        public BusinessType BusinessType { get; set; }

        public string RegistrationNumber { get; set; }

        public ProducerViewModel()
        {
        }

        public ProducerViewModel(Producer producer)
        {
            Address = new AddressViewModel(producer.Address, AddressTypeEnum.Producer);
            AreMultiple = producer.AreMultiple;
            Business = new BusinessViewModel(producer.BusinessName, producer.RegistrationNumber);
            Contact = new ContactViewModel(producer.Contact, AddressTypeEnum.Producer);
            IsAddedToAddressBook = producer.IsAddedToAddressBook;
            BusinessType = producer.Type;
        }
    }
}