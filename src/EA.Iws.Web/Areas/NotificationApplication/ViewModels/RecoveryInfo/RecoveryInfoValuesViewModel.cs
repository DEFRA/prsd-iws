namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.RecoveryInfo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.RecoveryInfo;
    using Prsd.Core.Helpers;
    using Prsd.Core.Validation;
    using Requests.RecoveryInfo;
    using Web.ViewModels.Shared;

    public class RecoveryInfoValuesViewModel : IValidatableObject
    {
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public bool IsDisposal { get; set; }

        [Required]
        public RadioButtonStringCollectionViewModel EstimatedUnit { get; set; }

        [Required]
        public RadioButtonStringCollectionViewModel CostUnit { get; set; }

        [RequiredIf("IsDisposal", true, "Please answer this question.")]
        public RadioButtonStringCollectionOptionalViewModel DisposalUnit { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Please enter the amount in GBP(£) for estimated value of the recoverable material.")]
        [RegularExpression(@"^[-]?\d+(\.\d{1,2})?", ErrorMessage = "The estimated amount must be a number with a maximum of 2 decimal places.")]
        [Display(Name = "Enter the amount in GBP(£)")]
        public decimal? EstimatedAmount { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Please enter the amount in GBP(£) for cost of recovery.")]
        [RegularExpression(@"^[-]?\d+(\.\d{1,2})?", ErrorMessage = "The cost amount must be a number with a maximum of 2 decimal places.")]
        [Display(Name = "Enter the amount in GBP(£)")]
        public decimal? CostAmount { get; set; }

        [DataType(DataType.Currency)]
        [RequiredIf("IsDisposal", true, "Please enter the amount in GBP(£) for cost of disposal of the non-recoverable fraction.")]
        [RegularExpression(@"^[-]?\d+(\.\d{1,2})?", ErrorMessage = "The cost of disposal amount must be a number with a maximum of 2 decimal places.")]
        [Display(Name = "Enter the amount in GBP(£)")]
        public decimal? DisposalAmount { get; set; }

        public AddRecoveryInfoToNotification ToRequest()
        {
            RecoveryInfoUnits estimatedUnit = (EstimatedUnit.SelectedValue == EnumHelper.GetDisplayName(RecoveryInfoUnits.Kilogram))
                                                ? RecoveryInfoUnits.Kilogram : RecoveryInfoUnits.Tonne;

            RecoveryInfoUnits costUnit = (CostUnit.SelectedValue == EnumHelper.GetDisplayName(RecoveryInfoUnits.Kilogram))
                                                ? RecoveryInfoUnits.Kilogram : RecoveryInfoUnits.Tonne;

            if (IsDisposal)
            {
                RecoveryInfoUnits disposalUnit = (DisposalUnit.SelectedValue == EnumHelper.GetDisplayName(RecoveryInfoUnits.Kilogram))
                                                    ? RecoveryInfoUnits.Kilogram : RecoveryInfoUnits.Tonne;

                return new AddRecoveryInfoToNotification(NotificationId, IsDisposal,
                            estimatedUnit, EstimatedAmount.GetValueOrDefault(),
                            costUnit, CostAmount.GetValueOrDefault(),
                            disposalUnit, DisposalAmount.GetValueOrDefault());
            }

            return new AddRecoveryInfoToNotification(NotificationId, IsDisposal,
                        estimatedUnit, EstimatedAmount.GetValueOrDefault(),
                        costUnit, CostAmount.GetValueOrDefault(), null, null);
        }

        public RecoveryInfoValuesViewModel()
        {
            EstimatedUnit = RadioButtonStringCollectionViewModel.CreateFromEnum<RecoveryInfoUnits>();
            CostUnit = RadioButtonStringCollectionViewModel.CreateFromEnum<RecoveryInfoUnits>();
            DisposalUnit = RadioButtonStringCollectionOptionalViewModel.CreateFromEnum<RecoveryInfoUnits>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (IsDisposal)
            {
                if (String.IsNullOrWhiteSpace(DisposalUnit.SelectedValue))
                {
                    results.Add(new ValidationResult("Please answer this question."));
                }

                if (!DisposalAmount.HasValue)
                {
                    results.Add(new ValidationResult("Please enter the amount in GBP(£) for cost of disposal of the non-recoverable fraction."));
                }
            }
            return results;
        }
    }
}