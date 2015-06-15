namespace EA.Iws.Web.ViewModels.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Requests.WasteType;

    public class WasteCodeViewModel
    {
        public IEnumerable<WasteCodeData> WasteCodes { get; set; }

        [Required]
        [Display(Name = "Basel annex VIII/IX or OECD code")]
        public string SelectedWasteCode { get; set; }

        public Guid NotificationId { get; set; }

        public IEnumerable<WasteCodeData> EcwCodes { get; set; }

        [Required]
        [Display(Name = "ECW Code")]
        public string SelectedEcwCode { get; set; }
    }
}