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
    using Views.WasteRecovery;

    public class RecoveryCostViewModel : IValidatableObject
    {
        public decimal PercentageRecoverable { get; set; }

        public ValuePerWeightUnits ShipmentInfoUnits { get; set; }

        public decimal EstimatedValueAmount { get; set; }

        public ValuePerWeightUnits EstimatedValueUnit { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(RecoveryCostResources))]
        [Required(ErrorMessageResourceName = "AmountRequired", ErrorMessageResourceType = typeof(RecoveryCostResources))]
        [IsValidNumber(maxPrecision: 12)]
        [IsValidMoneyDecimal]
        public string Amount { get; set; }

        [Display(Name = "SelectedUnits", ResourceType = typeof(RecoveryCostResources))]
        [Required(ErrorMessageResourceName = "UnitsRequired", ErrorMessageResourceType = typeof(RecoveryCostResources))]
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
            ValuePerWeightData recoveryCostData,
            ValuePerWeightUnits shipmentInfoUnits)
        {
            PercentageRecoverable = percentage;
            EstimatedValueAmount = estimatedValueData.Amount;
            EstimatedValueUnit = estimatedValueData.Unit;

            if (recoveryCostData != null)
            {
                Amount = recoveryCostData.Amount.ToString();
                SelectedUnits = recoveryCostData.Unit;
            }
            else
            {
                SelectedUnits = shipmentInfoUnits;
            }
        }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Amount.ToMoneyDecimal() < 0)
            {
                yield return new ValidationResult(RecoveryCostResources.AmountCannotBeNegative, new[] { "Amount" });
            }

            if (PercentageRecoverable < 0 
                || PercentageRecoverable > 100)
            {
                yield return new ValidationResult(RecoveryCostResources.InvalidData, new[] { "Amount" });
            }
        }
    }
}