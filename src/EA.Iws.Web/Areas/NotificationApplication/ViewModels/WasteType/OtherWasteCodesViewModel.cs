namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteType
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OtherWasteCodesViewModel
    {
        public Guid NotificationId { get; set; }

        [Display(Name = "National code in country of import")]
        public string ImportNationalCode { get; set; }

        [Display(Name = "National code in country of export")]
        public string ExportNationalCode { get; set; }

        [Display(Name = "Other codes")]
        public string OtherCode { get; set; }
    }
}