﻿namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.Carrier
{
    using System;
    using Core.Shared;
    using Requests.Carriers;
    using Web.ViewModels.Shared;

    public class AddCarrierViewModel
    {
        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public BusinessTypeViewModel Business { get; set; }

        public bool IsAddedToAddressBook { get; set; }

        public AddCarrierViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessTypeViewModel();
        }

        public AddCarrierToNotification ToRequest()
        {
            return new AddCarrierToNotification
            {
                NotificationId = NotificationId,
                Address = Address,
                Business = Business.ToBusinessInfoData(),
                Contact = Contact
            };
        }
    }
}