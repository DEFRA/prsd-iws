namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteType
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core.Validation;

    public class OtherWasteAdditionalInformationViewModel
    {
        public Guid NotificationId { get; set; }

        [RequiredIf("HasAttachement", false, ErrorMessage = "Please enter a description or check the box")]
        [StringLength(256, ErrorMessage = "The text cannot be longer than 256 characters")]
        public string Description { get; set; }

        public bool HasAttachement { get; set; }
    }
}