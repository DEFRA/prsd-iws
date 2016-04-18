namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.PaymentDetails
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Shared;
    using Infrastructure.Validation;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class PaymentDetailsViewModel : IValidatableObject
    {
        [Display(Name = "Date", ResourceType = typeof(PaymentDetailsViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "DateRequiredError", ErrorMessageResourceType = typeof(PaymentDetailsViewModelResources))]
        public OptionalDateInputViewModel Date { get; set; }

        [Display(Name = "PaymentMethod", ResourceType = typeof(PaymentDetailsViewModelResources))]
        public PaymentMethod PaymentMethod { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(PaymentDetailsViewModelResources))]
        [Required(ErrorMessageResourceName = "AmountRequiredError", ErrorMessageResourceType = typeof(PaymentDetailsViewModelResources))]
        public decimal? Amount { get; set; }

        [Display(Name = "ReceiptNumber", ResourceType = typeof(PaymentDetailsViewModelResources))]
        public string ReceiptNumber { get; set; }

        [Display(Name = "Comments", ResourceType = typeof(PaymentDetailsViewModelResources))]
        public string Comments { get; set; }
        
        public SelectList PaymentMethodsList
        {
            get
            {
                return new SelectList(EnumHelper.GetValues(typeof(PaymentMethod)), "Key", "Value");
            }
        }

        public decimal ChargeDue { get; set; }

        public PaymentDetailsViewModel()
        {
            Date = new OptionalDateInputViewModel(true);
            PaymentMethod = PaymentMethod.Cheque;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ReceiptNumber) && PaymentMethod == PaymentMethod.Cheque)
            {
                yield return new ValidationResult(PaymentDetailsViewModelResources.ReceiptNumberRequiredError, new[] { "ReceiptNumber" });
            }

            if (ReceiptNumber != null && ReceiptNumber.Length > 100)
            {
                yield return new ValidationResult(PaymentDetailsViewModelResources.ReceiptLengthError, new[] { "ReceiptNumber" });
            }

            if (Comments != null && Comments.Length > 500)
            {
                yield return new ValidationResult(PaymentDetailsViewModelResources.CommentsLengthError, new[] { "Comments" });
            }
        }
    }
}