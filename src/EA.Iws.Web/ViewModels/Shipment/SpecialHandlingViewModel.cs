namespace EA.Iws.Web.ViewModels.Shipment
{
    using System;
    using Prsd.Core.Validation;

    public class SpecialHandlingViewModel
    {
        public bool IsSpecialHandling { get; set; }

        public Guid NotificationId { get; set; }

        [RequiredIf("IsSpecialHandling", true, "Please enter special handling details")]
        public string SpecialHandlingDetails { get; set; }
    }
}