namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteRecovery
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Shared;
    using Infrastructure.Attributes;
    using Prsd.Core.Helpers;
    using Requests.WasteRecovery;

    public class RecoveryCostViewModel : IValidatableObject
    {
        public decimal PercentageRecoverable { get; set; }

        public decimal EstimatedValueAmount { get; set; }

        public ValuePerWeightUnits EstimatedValueUnit { get; set; }

        [Required(ErrorMessage = "Please enter a value")]
        [Display(Name = "Please enter £/kg or tonne")]
        [IsValidNumber(maxPrecision: 12, allowNegative: false)]
        [IsValidMoneyDecimal]
        public string Amount { get; set; }

        [Required]
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
            if (PercentageRecoverable < 0 
                || PercentageRecoverable > 100
                || EstimatedValueAmount < 0)
            {
                yield return new ValidationResult("Some of your data is invalid, please go back and re-enter your recovery information", new[] { "Amount" });
            }
        }
    }
}