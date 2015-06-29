namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteType
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OtherWasteViewModel
    {
        public Guid NotificationId { get; set; }

        [Required]
        public string Description { get; set; }
    }
}