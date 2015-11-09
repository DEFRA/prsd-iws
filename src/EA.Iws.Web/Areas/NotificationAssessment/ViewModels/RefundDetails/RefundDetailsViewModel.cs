namespace EA.Iws.Web.Areas.NotificationAssessment.ViewModels.RefundDetails
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using Infrastructure;
    using Infrastructure.Validation;
    using Prsd.Core;

    public class RefundDetailsViewModel : IValidatableObject
    {
        private const NumberStyles Style = NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint;

        public Guid NotificationId { get; set; }

        [Required(ErrorMessageResourceName = "AmountRefundedError", ErrorMessageResourceType = typeof(RefundDetailsViewModelResources))]
        [Display(Name = "AmountRefundedLabel", ResourceType = typeof(RefundDetailsViewModelResources))]
        [IsValidNumber(maxPrecision: 12)]
        [IsValidMoneyDecimal]
        public string Amount { get; set; }

        [Required(ErrorMessageResourceName = "DayError", ErrorMessageResourceType = typeof(RefundDetailsViewModelResources))]
        [Display(Name = "DayLabel", ResourceType = typeof(RefundDetailsViewModelResources))]
        [Range(1, 31, ErrorMessageResourceName = "DayError", ErrorMessageResourceType = typeof(RefundDetailsViewModelResources))]
        public int? Day { get; set; }

        [Required(ErrorMessageResourceName = "MonthError", ErrorMessageResourceType = typeof(RefundDetailsViewModelResources))]
        [Display(Name = "MonthLabel", ResourceType = typeof(RefundDetailsViewModelResources))]
        [Range(1, 12, ErrorMessageResourceName = "MonthError", ErrorMessageResourceType = typeof(RefundDetailsViewModelResources))]
        public int? Month { get; set; }

        [Required(ErrorMessageResourceName = "YearError", ErrorMessageResourceType = typeof(RefundDetailsViewModelResources))]
        [Display(Name = "YearLabel", ResourceType = typeof(RefundDetailsViewModelResources))]
        [Range(2015, 3000, ErrorMessageResourceName = "YearError", ErrorMessageResourceType = typeof(RefundDetailsViewModelResources))]
        public int? Year { get; set; }

        [Display(Name = "CommentsLabel", ResourceType = typeof(RefundDetailsViewModelResources))]
        public string Comments { get; set; }

        public DateTime Date()
        {
            DateTime date;
            SystemTime.TryParse(Year.GetValueOrDefault(), Month.GetValueOrDefault(), Day.GetValueOrDefault(), out date);

            return date;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Comments.Length > 500)
            {
                results.Add(new ValidationResult(RefundDetailsViewModelResources.CommentsLengthError, new[] { "Comments" }));
            }

            if (Amount.ToMoneyDecimal() < 0)
            {
                results.Add(new ValidationResult(RefundDetailsViewModelResources.AmountCannotBeNegative, new[] { "Amount" }));
            }

            return results;
        }
    }
}