namespace EA.Iws.Web.Areas.Movement.ViewModels.EditDate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EditDateViewModel
    {
        [Required]
        public int? Day { get; set; }
        [Required]
        public int? Month { get; set; }
        [Required]
        public int? Year { get; set; }

        public Dictionary<int, DateTime> PreviousDates { get; set; }
    }
}