namespace EA.Iws.Web.Areas.NotificationAssessment.ViewModels.PaymentDetails
{
    using System;
    using Prsd.Core.Helpers;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    public class PaymentDetailsViewModel : IValidatableObject
    {
        public PaymentDetailsViewModel() 
        {
            PaymentMethodsSelectList = new SelectList(EnumHelper.GetValues(typeof(PaymentMethods)), "Key", "Value");
        }

        private const NumberStyles Style = NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint;

        public Guid NotificationId { get; set; }

        [Required(ErrorMessageResourceName = "AmountPaidError", ErrorMessageResourceType = typeof(PaymentDetailsViewModelResources))]
        [Display(Name = "AmountPaidLabel", ResourceType = typeof(PaymentDetailsViewModelResources))]
        public string Amount { get; set; }

        [Display(Name = "PaymentMethodLabel", ResourceType = typeof(PaymentDetailsViewModelResources))]
        public PaymentMethods PaymentMethod { get; set; }

        public IEnumerable<SelectListItem> PaymentMethodsSelectList { get; set; }

        [Display(Name = "ReceiptNumberLabel", ResourceType = typeof(PaymentDetailsViewModelResources))]
        public string Receipt { get; set; }

        [Required(ErrorMessageResourceName = "DayError", ErrorMessageResourceType = typeof(PaymentDetailsViewModelResources))]
        [Display(Name = "DayLabel", ResourceType = typeof(PaymentDetailsViewModelResources))]
        [Range(1, 31, ErrorMessageResourceName = "DayError", ErrorMessageResourceType = typeof(PaymentDetailsViewModelResources))]
        public int? Day { get; set; }

        [Required(ErrorMessageResourceName = "MonthError", ErrorMessageResourceType = typeof(PaymentDetailsViewModelResources))]
        [Display(Name = "MonthLabel", ResourceType = typeof(PaymentDetailsViewModelResources))]
        [Range(1, 12, ErrorMessageResourceName = "MonthError", ErrorMessageResourceType = typeof(PaymentDetailsViewModelResources))]
        public int? Month { get; set; }

        [Required(ErrorMessageResourceName = "YearError", ErrorMessageResourceType = typeof(PaymentDetailsViewModelResources))]
        [Display(Name = "YearLabel", ResourceType = typeof(PaymentDetailsViewModelResources))]
        [Range(2015, 3000, ErrorMessageResourceName = "YearError", ErrorMessageResourceType = typeof(PaymentDetailsViewModelResources))]
        public int? Year { get; set; }

        [Display(Name = "CommentsLabel", ResourceType = typeof(PaymentDetailsViewModelResources))]
        public string Comments { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!AmountValid())
            {
                yield return new ValidationResult(string.Format(PaymentDetailsViewModelResources.AmountDecimalPlaceError, 2, new[] { "Amount" }));
            }

            if (Receipt.Length > 100)
            {
                yield return new ValidationResult(PaymentDetailsViewModelResources.ReceiptLengthError, new[] { "Receipt" });
            }

            if (Comments.Length > 500)
            {
                yield return new ValidationResult(PaymentDetailsViewModelResources.CommentsLengthError, new[] { "Comments" });
            }
        }

        public bool AmountValid()
        {
            decimal amount;
            return decimal.TryParse(Amount, Style, CultureInfo.CurrentCulture, out amount) && decimal.Round(amount, 2) == amount;
        }
    }
}