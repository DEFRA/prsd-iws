namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.PaymentDetails
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Web.Mvc;
    using Infrastructure;
    using Infrastructure.Validation;
    using Prsd.Core;
    using Prsd.Core.Helpers;

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
        [IsValidMoneyDecimal]
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

        public DateTime Date()
        {
            DateTime date;
            SystemTime.TryParse(Year.GetValueOrDefault(), Month.GetValueOrDefault(), Day.GetValueOrDefault(), out date);

            return date;
        }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Receipt != null && Receipt.Length > 100)
            {
                results.Add(new ValidationResult(PaymentDetailsViewModelResources.ReceiptLengthError, new[] { "Receipt" }));
            }

            if (string.IsNullOrWhiteSpace(Receipt) && PaymentMethod == PaymentMethods.Cheque)
            {
                results.Add(new ValidationResult(PaymentDetailsViewModelResources.ReceiptRequiredError, new[] { "Receipt" }));
            }
            
            if (Comments != null && Comments.Length > 500)
            {
                results.Add(new ValidationResult(PaymentDetailsViewModelResources.CommentsLengthError, new[] { "Comments" }));
            }

            if (Amount.ToMoneyDecimal() < 0)
            {
                results.Add(new ValidationResult(PaymentDetailsViewModelResources.AmountCannotBeNegative, new[] { "Amount" }));
            }

            return results;
        }
    }
}