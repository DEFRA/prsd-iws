namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteRecovery
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Shared;
    using Infrastructure;
    using Infrastructure.Validation;
    using Prsd.Core.Helpers;
    using Requests.WasteRecovery;

    public class RecoveryCostViewModel : IValidatableObject
    {
        public decimal PercentageRecoverable { get; set; }

        public decimal EstimatedValueAmount { get; set; }

        public ValuePerWeightUnits EstimatedValueUnit { get; set; }

        [Display(Name = "Please enter £/kg or tonne")]
        [Required(ErrorMessage = "Please enter the amount in GBP(£) for cost of recovery")]
        [IsValidNumber(maxPrecision: 12)]
        [IsValidMoneyDecimal]
        public string Amount { get; set; }

        [Required(ErrorMessage = "Please select the units")]
        public ValuePerWeightUnits? SelectedUnits { get; set; }

        public SelectList UnitSelectList
        {
            get
            {
                var units = Enum.GetValues(typeof(ValuePerWeightUnits))
                    .Cast<ValuePerWeightUnits>()
                    .Select(u => new
                    {
                        Key = EnumHelper.GetDisplayName(u),
                        Value = (int)u
                    }).ToList();

                return new SelectList(units, "Value", "Key", SelectedUnits);
            }
        }

        public RecoveryCostViewModel()
        {
        }

        public RecoveryCostViewModel(decimal percentage, 
            ValuePerWeightData estimatedValueData, 
            ValuePerWeightData recoveryCostData)
        {
            PercentageRecoverable = percentage;
            EstimatedValueAmount = estimatedValueData.Amount;
            EstimatedValueUnit = estimatedValueData.Unit;

            if (recoveryCostData != null)
            {
                Amount = recoveryCostData.Amount.ToString();
                SelectedUnits = recoveryCostData.Unit;
            }
        }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Amount.ToMoneyDecimal() < 0)
            {
                yield return new ValidationResult("The amount entered cannot be negative", new[] { "Amount" });
            }

            if (PercentageRecoverable < 0 
                || PercentageRecoverable > 100
                || EstimatedValueAmount < 0)
            {
                yield return new ValidationResult("Some of your data is invalid, please go back and re-enter your recovery information", new[] { "Amount" });
            }
        }
    }
}