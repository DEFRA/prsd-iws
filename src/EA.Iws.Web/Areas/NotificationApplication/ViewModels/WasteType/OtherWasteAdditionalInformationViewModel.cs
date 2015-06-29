namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteType
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OtherWasteAdditionalInformationViewModel
    {
        public Guid NotificationId { get; set; }

        [Required]
        public string Description { get; set; }

        public bool HasAttachement { get; set; }
    }
}