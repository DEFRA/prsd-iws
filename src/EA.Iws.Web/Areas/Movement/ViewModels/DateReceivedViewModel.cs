namespace EA.Iws.Web.Areas.Movement.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DateReceivedViewModel
    {
        [Required]
        [Display(Name = "Day")]
        public int? Day { get; set; }

        [Required]
        [Display(Name = "Month")]
        public int? Month { get; set; }

        [Required]
        [Display(Name = "Year")]
        public int? Year { get; set; }

        public DateReceivedViewModel()
        {
        }

        public DateReceivedViewModel(DateTime? dateReceived)
        {
            if (dateReceived.HasValue)
            {
                Day = dateReceived.Value.Day;
                Month = dateReceived.Value.Month;
                Year = dateReceived.Value.Year;
            }
        }

        public DateTime GetDateReceived()
        {
            if (Day.HasValue && Month.HasValue && Year.HasValue)
            {
                return new DateTime(Year.Value, Month.Value, Day.Value);
            }
            else
            {
                throw new InvalidOperationException("Date not valid");
            }
        }
    }
}