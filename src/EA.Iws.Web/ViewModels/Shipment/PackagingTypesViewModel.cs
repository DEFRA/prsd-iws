namespace EA.Iws.Web.ViewModels.Shipment
{
    using System;
    using Prsd.Core.Validation;
    using Shared;

    public class PackagingTypesViewModel
    {
        public CheckBoxCollectionViewModel PackagingTypes { get; set; }

        public Guid NotificationId { get; set; }

        public bool OtherSelected { get; set; }

        [RequiredIf("OtherSelected", true, "Please enter a description")]
        public string OtherDescription { get; set; }
    }
}