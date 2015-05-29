namespace EA.Iws.Web.ViewModels.Shipment
{
    using System;
    using Shared;

    public class PackagingTypesViewModel
    {
        public CheckBoxCollectionViewModel PackagingTypes { get; set; }

        public Guid NotificationId { get; set; }

        public string OtherDescription { get; set; }
    }
}