namespace EA.Iws.Web.Areas.Movement.ViewModels
{
    using System;
    using Core.Shared;
    using System.ComponentModel.DataAnnotations;

    public class DateCompleteViewModel
    {
        public NotificationType NotificationType { get; set; }

        [Required]
        public int? Day { get; set; }
        [Required]
        public int? Month { get; set; }
        [Required]
        public int? Year { get; set; }

        private DateTime GetDateComplete()
        {
            throw new NotImplementedException();
        }
    }
}