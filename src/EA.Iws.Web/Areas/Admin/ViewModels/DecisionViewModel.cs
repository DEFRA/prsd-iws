namespace EA.Iws.Web.Areas.Admin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class DecisionViewModel : IValidatableObject
    {
        public DecisionViewModel()
        {
            DecisionMadeDate = new OptionalDateInputViewModel(true);
            ConsentValidFromDate = new OptionalDateInputViewModel(true);
            ConsentValidToDate = new OptionalDateInputViewModel(true);
        }

        public Guid NotificationId { get; set; }

        public string ConditionsOfConsent { get; set; }

        public SelectList DecisionTypes { get; set; }

        public string DecisionType { get; set; }

        public OptionalDateInputViewModel DecisionMadeDate { get; set; }

        public OptionalDateInputViewModel ConsentValidFromDate { get; set; }

        public OptionalDateInputViewModel ConsentValidToDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ConsentValidFromDate.IsCompleted && ConsentValidToDate.IsCompleted)
            {
                if (ConsentValidFromDate.AsDateTime() >= ConsentValidToDate.AsDateTime())
                {
                    yield return new ValidationResult("The consent 'valid from' date must be before the 'valid to' date", new[] { "ConsentValidToDate" });
                }
            }
        }
    }
}