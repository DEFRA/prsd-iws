namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.CustomsOffice
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class CustomsOfficeViewModel
    {
        [Required]
        [StringLength(1024)]
        public string Name { get; set; }

        [Required]
        [StringLength(4000)]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        public SelectList Countries { get; set; }

        [Required]
        [Display(Name = "Country")]
        public Guid? SelectedCountry { get; set; }
    }
}