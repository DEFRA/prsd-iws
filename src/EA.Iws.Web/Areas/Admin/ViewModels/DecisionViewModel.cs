namespace EA.Iws.Web.Areas.Admin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class DecisionViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public string ConditionsOfConsent { get; set; }

        public SelectList DecisionTypes { get; set; }

        public string DecisionType { get; set; }

        //----------------Guarantee Received-----------

        [Display(Name = "Day")]
        public int? DecisionMadeDay { get; set; }

        [Display(Name = "Month")]
        public int? DecisionMadeMonth { get; set; }

        [Display(Name = "Year")]
        public int? DecisionMadeYear { get; set; }

        public bool DecisionMadeComplete
        {
            get { return DateComplete(DecisionMadeDay, DecisionMadeMonth, DecisionMadeYear); }
        }

        //----------------Decision Required-----------

        [Display(Name = "Day")]
        public int? ConsentValidFromDay { get; set; }

        [Display(Name = "Month")]
        public int? ConsentValidFromMonth { get; set; }

        [Display(Name = "Year")]
        public int? ConsentValidFromYear { get; set; }

        public bool ConsentValidFromComplete
        {
            get { return DateComplete(ConsentValidFromDay, ConsentValidFromMonth, ConsentValidFromYear); }
        }

        //----------------Guarantee Required-----------

        [Display(Name = "Day")]
        public int? ConsentValidToDay { get; set; }

        [Display(Name = "Month")]
        public int? ConsentValidToMonth { get; set; }

        [Display(Name = "Year")]
        public int? ConsentValidToYear { get; set; }

        public bool GuaranteeRequiredComplete
        {
            get { return (DateComplete(ConsentValidToDay, ConsentValidToMonth, ConsentValidToYear)); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ValidateDate(DecisionMadeDay, DecisionMadeMonth, DecisionMadeYear))
            {
                yield return new ValidationResult("Please provide a valid 'decision made' date", new[] { "DecisionMadeDay" });
            }

            if (!ValidateDate(ConsentValidFromDay, ConsentValidFromMonth, ConsentValidFromYear))
            {
                yield return new ValidationResult("Please provide a valid 'consent valid from' date", new[] { "ConsentValidFromDay" });
            }

            if (!ValidateDate(ConsentValidToDay, ConsentValidToMonth, ConsentValidToYear))
            {
                yield return new ValidationResult("Please provide a valid 'consent valid to' date", new[] { "ConsentValidToDay" });
            }

            if (DateComplete(ConsentValidFromDay, ConsentValidFromMonth, ConsentValidFromYear) && DateComplete(ConsentValidToDay, ConsentValidToMonth, ConsentValidToYear))
            {
                if (new DateTime(ConsentValidFromYear.Value, ConsentValidFromMonth.Value, ConsentValidFromDay.Value) >= new DateTime(ConsentValidToYear.Value, ConsentValidToMonth.Value, ConsentValidToDay.Value))
                {
                    yield return new ValidationResult("The consent 'valid from' date must be before the 'valid to' date", new[] { "ConsentValidToDay" });                    
                }
            }
        }

        private bool ValidateDate(int? day, int? month, int? year)
        {
            if (day.HasValue || month.HasValue || year.HasValue)
            {
                if (!(day.HasValue && month.HasValue && year.HasValue))
                {
                    return false;
                }
                return day.Value <= 31 && day.Value >= 1 && month.Value <= 12 && month.Value >= 1 && year.Value <= 9000 && year.Value >= 1900;
            }
            return true;
        }

        private bool DateComplete(int? day, int? month, int? year)
        {
            if ((day.HasValue || month.HasValue || year.HasValue) && (day.HasValue && month.HasValue && year.HasValue))
            {
                return day.Value <= 31 && day.Value >= 1 && month.Value <= 12 && month.Value >= 1 && year.Value <= 9000 && year.Value >= 1900;
            }
            return false;
        }
    }
}