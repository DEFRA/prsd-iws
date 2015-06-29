namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteCodes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OtherWasteCodesViewModel
    {
        public Guid NotificationId { get; set; }

        [Required(ErrorMessage = "Enter the relevant code or not applicable")]
        [Display(Name = "National code in country of import")]
        public string ImportNationalCode { get; set; }

        [Required(ErrorMessage = "Enter the relevant code or not applicable")]
        [Display(Name = "National code in country of export")]
        public string ExportNationalCode { get; set; }

        [Display(Name = "Other codes")]
        public string OtherCode { get; set; }
    }
}