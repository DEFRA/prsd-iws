﻿namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.PaymentDetails
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Shared;
    using Infrastructure;
    using Infrastructure.Validation;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class PaymentDetailsViewModel : IValidatableObject
    {
        public PaymentDetailsViewModel() 
        {
            PaymentMethodsSelectList = new SelectList(EnumHelper.GetValues(typeof(PaymentMethod)), "Key", "Value");
            PaymentDate = new OptionalDateInputViewModel(true);
        }

        public Guid NotificationId { get; set; }

        [Required(ErrorMessageResourceName = "AmountPaidError", ErrorMessageResourceType = typeof(PaymentDetailsViewModelResources))]
        [Display(Name = "AmountPaidLabel", ResourceType = typeof(PaymentDetailsViewModelResources))]
        [IsValidMoneyDecimal]
        public string PaymentAmount { get; set; }

        [Display(Name = "PaymentMethodLabel", ResourceType = typeof(PaymentDetailsViewModelResources))]
        public PaymentMethod PaymentMethod { get; set; }

        public IEnumerable<SelectListItem> PaymentMethodsSelectList { get; set; }

        [Display(Name = "ReceiptNumberLabel", ResourceType = typeof(PaymentDetailsViewModelResources))]
        public string Receipt { get; set; }

        [Display(Name = "DateLabel", ResourceType = typeof(PaymentDetailsViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "DateRequiredError", ErrorMessageResourceType = typeof(PaymentDetailsViewModelResources))]
        public OptionalDateInputViewModel PaymentDate { get; set; }
        
        [Display(Name = "CommentsLabel", ResourceType = typeof(PaymentDetailsViewModelResources))]
        [Required(ErrorMessageResourceName = "CommentsRequired", ErrorMessageResourceType = typeof(PaymentDetailsViewModelResources))]
        public string PaymentComments { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Receipt != null && Receipt.Length > 100)
            {
                results.Add(new ValidationResult(PaymentDetailsViewModelResources.ReceiptLengthError, new[] { "Receipt" }));
            }

            if (string.IsNullOrWhiteSpace(Receipt) && PaymentMethod == PaymentMethod.Cheque)
            {
                results.Add(new ValidationResult(PaymentDetailsViewModelResources.ReceiptRequiredError, new[] { "Receipt" }));
            }
            
            if (PaymentComments != null && PaymentComments.Length > 500)
            {
                results.Add(new ValidationResult(PaymentDetailsViewModelResources.CommentsLengthError, new[] { "Comments" }));
            }

            if (PaymentAmount.ToMoneyDecimal() < 0)
            {
                results.Add(new ValidationResult(PaymentDetailsViewModelResources.AmountCannotBeNegative, new[] { "PaymentAmount" }));
            }

            return results;
        }
    }
}