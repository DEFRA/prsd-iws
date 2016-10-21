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
            Date = new OptionalDateInputViewModel(true);
        }

        public Guid NotificationId { get; set; }

        public decimal Limit { get; set; }

        public DateTime? FirstPaymentReceivedDate { get; set; }

        [Required(ErrorMessageResourceName = "AmountRefundedError",
            ErrorMessageResourceType = typeof(RefundDetailsViewModelResources))]
        [Display(Name = "AmountRefundedLabel", ResourceType = typeof(RefundDetailsViewModelResources))]
        [IsValidMoneyDecimal]
        public string Amount { get; set; }

        [RequiredDateInput(ErrorMessageResourceName = "DateRequiredError",
            ErrorMessageResourceType = typeof(RefundDetailsViewModelResources))]
        [Display(Name = "Date", ResourceType = typeof(RefundDetailsViewModelResources))]
        public OptionalDateInputViewModel Date { get; set; }

        [Display(Name = "CommentsLabel", ResourceType = typeof(RefundDetailsViewModelResources))]
        public string Comments { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Comments != null && Comments.Length > 500)
            {
                results.Add(new ValidationResult(RefundDetailsViewModelResources.CommentsLengthError,
                    new[] { "Comments" }));
            }

            if (Amount.ToMoneyDecimal() < 0)
            {
                results.Add(new ValidationResult(RefundDetailsViewModelResources.AmountCannotBeNegative,
                    new[] { "Amount" }));
            }

            if (Amount.ToMoneyDecimal() > Limit)
            {
                results.Add(
                    new ValidationResult(string.Format(RefundDetailsViewModelResources.AmountCannotExceedLimit, Limit),
                        new[] { "Amount" }));
            }

            if (Date.AsDateTime().HasValue)
            {
                if (Date.AsDateTime().Value > SystemTime.UtcNow.Date)
                {
                    results.Add(
                        new ValidationResult(RefundDetailsViewModelResources.DateNotInFuture,
                            new[] { "Date" }));
                }

                if (FirstPaymentReceivedDate.HasValue)
                {
                    if (Date.AsDateTime().Value < FirstPaymentReceivedDate.Value)
                    {
                        results.Add(
                            new ValidationResult(RefundDetailsViewModelResources.DateNotBeforeFirstPayment,
                                new[] { "Date" }));
                    }    
                }
                else
                {
                    results.Add(
                            new ValidationResult(RefundDetailsViewModelResources.NoPaymentsMade,
                                new[] { "Date" }));
                }
            }

            return results;
        }
    }
}