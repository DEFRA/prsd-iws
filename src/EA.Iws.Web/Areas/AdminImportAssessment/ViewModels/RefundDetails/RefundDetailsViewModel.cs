namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.RefundDetails
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Infrastructure;
    using Infrastructure.Validation;
    using Prsd.Core;
    using Web.ViewModels.Shared;

    public class RefundDetailsViewModel : IValidatableObject
    {
        public RefundDetailsViewModel()
        {
            RefundDate = new OptionalDateInputViewModel(true);
        }

        public Guid NotificationId { get; set; }

        public decimal Limit { get; set; }

        public DateTime? FirstPaymentReceivedDate { get; set; }

        [Required(ErrorMessageResourceName = "AmountRefundedError",
            ErrorMessageResourceType = typeof(RefundDetailsViewModelResources))]
        [Display(Name = "AmountRefundedLabel", ResourceType = typeof(RefundDetailsViewModelResources))]
        [IsValidMoneyDecimal]
        public string RefundAmount { get; set; }

        [RequiredDateInput(ErrorMessageResourceName = "DateRequiredError",
            ErrorMessageResourceType = typeof(RefundDetailsViewModelResources))]
        [Display(Name = "Date", ResourceType = typeof(RefundDetailsViewModelResources))]
        public OptionalDateInputViewModel RefundDate { get; set; }

        [Display(Name = "CommentsLabel", ResourceType = typeof(RefundDetailsViewModelResources))]
        [Required(ErrorMessageResourceName = "CommentsRequired", ErrorMessageResourceType = typeof(RefundDetailsViewModelResources))]
        public string RefundComments { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (RefundComments != null && RefundComments.Length > 500)
            {
                results.Add(new ValidationResult(RefundDetailsViewModelResources.CommentsLengthError,
                    new[] { "RefundComments" }));
            }

            if (RefundAmount.ToMoneyDecimal() < 0)
            {
                results.Add(new ValidationResult(RefundDetailsViewModelResources.AmountCannotBeNegative,
                    new[] { "RefundAmount" }));
            }

            if (RefundAmount.ToMoneyDecimal() > Limit)
            {
                results.Add(
                    new ValidationResult(string.Format(RefundDetailsViewModelResources.AmountCannotExceedLimit, Limit),
                        new[] { "RefundAmount" }));
            }

            if (RefundDate.AsDateTime().HasValue)
            {
                if (RefundDate.AsDateTime().Value > SystemTime.UtcNow.Date)
                {
                    results.Add(
                        new ValidationResult(RefundDetailsViewModelResources.DateNotInFuture,
                            new[] { "RefundDate" }));
                }

                if (FirstPaymentReceivedDate.HasValue)
                {
                    if (RefundDate.AsDateTime().Value < FirstPaymentReceivedDate.Value)
                    {
                        results.Add(
                            new ValidationResult(RefundDetailsViewModelResources.DateNotBeforeFirstPayment,
                                new[] { "RefundDate" }));
                    }    
                }
                else
                {
                    results.Add(
                            new ValidationResult(RefundDetailsViewModelResources.NoPaymentsMade,
                                new[] { "RefundDate" }));
                }
            }

            return results;
        }
    }
}