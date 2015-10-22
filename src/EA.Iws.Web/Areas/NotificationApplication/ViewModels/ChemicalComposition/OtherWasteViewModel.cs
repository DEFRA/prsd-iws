namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.ChemicalComposition
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OtherWasteViewModel
    {
        public Guid NotificationId { get; set; }

        [Required]
        [StringLength(70, ErrorMessage = "Please limit your answer to 70 characters or less")]
        public string Description { get; set; }
    }
}